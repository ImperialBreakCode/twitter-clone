$(document).ready(() => {

    $(".profiles button").click(e => {

        let id = e.target.id.split("-");

        $.ajax({
            type: "POST",
            url: `/User/${id[0]}`,
            data: { username: id[1] },
            success: () => {

                if (id[0] == "UnfollowUser") {
                    e.target.classList.remove("following");
                    e.target.innerHTML = "Follow";
                }
                else if (id[0] == "FollowUser") {
                    e.target.classList.add("following");
                    e.target.innerHTML = "Following";
                }
            }
        });

    });

});