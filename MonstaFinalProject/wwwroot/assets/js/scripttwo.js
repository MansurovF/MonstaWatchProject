//  const sliderContent = document.querySelectorAll('.slider-content');
// const sliderButtons = document.querySelectorAll('.slider-button');


// sliderButtons.forEach((button) => {
//   button.addEventListener('click', () => {
//     console.log("success");
//     const slideIndex = button.getAttribute('data-slide');
//     sliderButtons.forEach((button) => button.classList.remove('active'));
//     sliderContent.forEach((content) => content.classList.remove('active'));
//     button.classList.add('active');
//     sliderContent[slideIndex].classList.add('active');
//   });
// });
//click function
var div = document.getElementById('Countryadres');
var display = 0;
let search = document.querySelector('#SearchIcon');
let srcdiv = document.querySelector('#Search-div');

let sidebar = document.querySelector('#Sidebar');
let sidebar_open = document.querySelector('#Sidebar-open');
let sidebar_close = document.querySelector('#Sidebar-close');

//search.addEventListener('click', function() {
//    srcdiv.classList.toggle('active');
//})

sidebar_open.addEventListener('click', function() {
    sidebar.classList.add('active-sidebar');
})

sidebar_close.addEventListener('click', function() {
    sidebar.classList.remove('active-sidebar');
})

let goup = document.querySelector('#Up');
$(document).scroll(function() {
  var y = $(this).scrollTop();
  if (y > 20) {
    goup.style.visibility = 'visible';
    goup.style.opacity = '1';
  } else {
    goup.style.visibility = 'hidden';
    goup.style.opacity = '0';
  }
});

goup.addEventListener('click', function(){
  window.scrollTo({top: 0, behavior: 'smooth'});
})

if (localStorage.getItem('basket') === null) {
  localStorage.setItem('basket', JSON.stringify([]));
}

