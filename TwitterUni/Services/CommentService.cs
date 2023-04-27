using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.Entities;
using TwitterUni.Data.UnitOfWork;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, Mapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public CommentData? CreateComment(CommentData commentData, string userName, string tweetId)
        {
            User? user = _unitOfWork.UserRepository.GetByUsername(userName);
            Tweet? tweet = _unitOfWork.TweetRepository.GetOne(tweetId);

            if (tweet is not null && user is not null)
            {
                Comment comment = new Comment();
                _mapper.Map(commentData, comment);

                user.Comments.Add(comment);
                tweet.Comments.Add(comment);

                _unitOfWork.Commit();

                return _mapper.Map<CommentData>(comment);
            }

            return null;
        }

        public void DeleteComment(string commentId)
        {
            _unitOfWork.CommentRepository.DeleteOne(commentId);
            _unitOfWork.Commit();
        }

        public CommentData? GetComment(string commentId)
        {
            Comment? comment = _unitOfWork.CommentRepository.GetOne(commentId);
            CommentData? commentData = null;

            if (comment != null)
            {
                _mapper.Map(comment, commentData);
            }

            return commentData;
        }

        public ICollection<CommentData> GetTweetComments(string tweetId)
        {
            IQueryable<Comment> comments = _unitOfWork.CommentRepository.GetAll()
                .Where(c => c.ParentTweet.Id == tweetId)
                .Include(c => c.Author)
                .Include(c => c.Likes);

            List<CommentData> commentDatas = new List<CommentData>();

            foreach (Comment comment in comments)
            {
                commentDatas.Add(_mapper.Map<CommentData>(comment));
            }

            return commentDatas;
        }

        public void UpdateComment(CommentData commentData)
        {
            Comment? comment = _unitOfWork.CommentRepository.GetOne(commentData.Id);

            if (comment is not null)
            {
                _mapper.Map(commentData, comment);
                _unitOfWork.Commit();
            }
        }
    }
}
