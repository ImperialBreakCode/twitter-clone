$(document).ready(() => {

    $(".profiles button").click(e => {

        let id = e.target.id.split("-");

        $.ajax({
            type: `${id[0] == 'FollowUser' ? 'POST': 'DELETE'}`,
            url: `/User/${id[0]}`,
            data: { username: id[1] },
            success: () => {

                if (id[0] == "UnfollowUser") {
                    e.target.classList.remove("following");
                    e.target.innerHTML = "Follow";
                    e.target.id = `FollowUser-${id[1]}`;
                }
                else if (id[0] == "FollowUser") {
                    e.target.classList.add("following");
                    e.target.innerHTML = "Following";
                    e.target.id = `UnfollowUser-${id[1]}`;
                }
            }
        });

    });

});