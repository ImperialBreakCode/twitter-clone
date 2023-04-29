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
            Comment? comment = _unitOfWork.CommentRepository.GetAll()
                .Where(c => c.Id == commentId).Include(c => c.Author).First();

            if (comment != null)
            {
                return _mapper.Map<CommentData>(comment);
            }

            return null;
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

        public bool LikeComment(string commentId, string username)
        {
            Comment? comment = _unitOfWork.CommentRepository.GetOne(commentId);
            User? user = _unitOfWork.UserRepository.GetByUsername(username);

            if (user is not null && comment is not null)
            {
                comment.Likes.Add(user);
                _unitOfWork.Commit();
            }

            return user is not null && comment is not null;
        }

        public bool UnlikeComment(string commentId, string username)
        {
            Comment? comment = _unitOfWork.CommentRepository.GetAll()
                .Where(c => c.Id == commentId)
                .Include(c => c.Likes).First();
            User? user = _unitOfWork.UserRepository.GetByUsername(username);

            if (user is not null && comment is not null)
            {
                comment.Likes.Remove(user);
                _unitOfWork.Commit();
            }

            return user is not null && comment is not null;
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
