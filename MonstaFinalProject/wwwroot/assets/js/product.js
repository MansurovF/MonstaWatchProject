let review=document.querySelector('.review')
let reviewBtn=document.querySelector('.review-btn')
let desc=document.querySelector('#descBtn')
let descBtn=document.querySelector('.desc-btn')

reviewBtn.addEventListener("click",function(){
    reviewBtn.classList.add('active')
    desc.classList.add('hide');
    review.classList.remove('hide');
    if(reviewBtn.classList.contains('active')){
        reviewBtn.classList.add('yellow')
        descBtn.classList.remove('active')
        descBtn.classList.remove('yellow')
    }

})

descBtn.addEventListener("click",function(){
    descBtn.classList.add('active')
    review.classList.add('hide')
    desc.classList.remove('hide')
    if(descBtn.classList.contains('active')){
        descBtn.classList.add('yellow')
        reviewBtn.classList.remove('active')
        reviewBtn.classList.remove('yellow')
    }

})
