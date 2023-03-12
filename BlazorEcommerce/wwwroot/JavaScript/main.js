alert("Heloooooooo");
console.log("this is working just fine");
let taps = document.querySelectorAll("li .tap");
let tapsArray = Array.from(taps);
tapsArray.forEach((ele) => {
    ele.addEventListener("click", function (e) {

        tapsArray.forEach((ele) => {
            ele.classList.remove("active");
        })
        e.currentTarget.classList.add("active");
    })
})

var collapsibles = document.querySelectorAll(".collapsible");
collapsibles.forEach((item) =>
    item.addEventListener("click", function () {
        alert("Categoryyyyy");
      this.classList.toggle("collapsible--expanded");
      
  })
);
var cart = document.querySelector(".cart");
cart.addEventListener("click", function () {
    this.classList.toggle("toggle-cart");
  })

var mainImg = document.getElementById('main-img');
//var m = mainImg.src;
var bottomImg = document.getElementsByClassName('bottom-img');
let imgs = document.querySelectorAll(".bottom-img-container img");
let imsgArray = Array.from(imgs);
console.log(imsgArray);
imsgArray.forEach((ele) => {
    ele.addEventListener("click", function (e) {
        mainImg.src = e.currentTarget.src;
    })
})