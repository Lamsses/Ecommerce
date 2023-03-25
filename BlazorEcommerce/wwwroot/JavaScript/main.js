
//console.log("this is working just fine");
function SelectTap() {
    let taps = document.querySelectorAll("li .tap");
    let tapsArray = Array.from(taps);
    tapsArray.forEach((ele) => {
        ele.addEventListener("click",
            function(e) {

                tapsArray.forEach((ele) => {
                    ele.classList.remove("active");
                });
                e.currentTarget.classList.add("active");
            });
    });

}

function ShowCategory() {
    const collapsible = document.querySelector(".collapsible");
    collapsible.classList.toggle("collapsible--expanded");
//    console.log(collapsible);
//    collapsible.addEventListener("click",
//        function() {
//            this.classList.toggle("collapsible--expanded");
//
//        });

}
//let collapsibles = document.querySelectorAll(".collapsible");
//collapsibles.forEach((item) =>
//    item.addEventListener("click",
//        function () {
//            this.classList.toggle("collapsible--expanded");
//
//        })
//);

//var mainImg = document.getElementById('main-img');
////var m = mainImg.src;
//var bottomImg = document.getElementsByClassName('bottom-img');
//let imgs = document.querySelectorAll(".bottom-img-container img");
//let imsgArray = Array.from(imgs);
//console.log(imsgArray);
//imsgArray.forEach((ele) => {
//    ele.addEventListener("click",
//        function(e) {
//            mainImg.src = e.currentTarget.src;
//        });
//});