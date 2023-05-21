$(document).ready(function(){
    const rangeInput = document.querySelectorAll(".range-input input");
    const priceInput = document.querySelectorAll(".price-input .value");
    const range = document.querySelector(".slider .progress");
    let priceGap = 0;
 
    rangeInput.forEach(input =>{
        input.addEventListener("input", e =>{
            let minVal = parseInt(rangeInput[0].value);
            let maxVal = parseInt(rangeInput[1].value);
            if((maxVal - minVal) < priceGap){
                if(e.target.className === "range-min"){
                    rangeInput[0].value = maxVal - priceGap
                }else{
                    rangeInput[1].value = minVal + priceGap;
                }
            }else{
                priceInput[0].innerHTML = '$' + minVal;
                priceInput[1].innerHTML = '$' + maxVal;
                range.style.left = ((minVal / rangeInput[0].max) * 100 - 4.5) + "%";
                range.style.right = 105 - (maxVal / rangeInput[1].max) * 101 + "%";
            }
            minPrice = minVal;
            maxPrice = maxVal;
        });
   
        
        // input.addEventListener('change', function() {
        //     fetch('./products.json')
        //     .then(res => res.json())
        //     .then(data => {
        //         let html = '';
        //         let count = 0;
        //         data.forEach(item => {
        //             if(((page == 1 && count < 12) || (page == 2 && count < 24)) &&
        //                 (item.price >= minPrice && item.price <= maxPrice) && 
        //                 ( (category == '') || (category == item.category) ) && 
        //                 ( (bottlesize == '') || (bottlesize == item.bottlesize) ) && 
        //                 ( (vintage == '') || (vintage == item.vintage) ) 
        //                 ){
        //                 html += `
        //             <div id="${item.id}" class="productitem ${colname}">
        //             <a href="productdetail.html">
        //             <div class="box">
        //                 <div class="img-div">
        //                     <img src=${item.image} alt="">
        //                     <img src=${item.imageabs} alt="">
        //                     ${item.hot == 'true' ? '<div class="hot"><h1>Hot</h1></div>' : ''} 
        //                     ${item.discount != 'none' ? '<div class="discount"><h1>-'+ item.discount +'%</h1></div>' : ''} 
        //                     <div class="bag">
        //                         <i class="fa-solid fa-bag-shopping"></i>
        //                     </div>
        //                     <div class="heart">
        //                         <i class="fa-regular fa-heart"></i>
        //                     </div>
        //                     <div class="search">
        //                         <i class="fa-solid fa-magnifying-glass"></i>
        //                     </div>
        //                 </div>
        //                 <div class="body-div">
        //                     <div class="rating">
        //                         <i class="fa-${item.review != '0' ? 'solid' : 'regular'} fa-star"></i>
        //                         <i class="fa-${item.review != '0' ? 'solid' : 'regular'} fa-star"></i>
        //                         <i class="fa-${item.review != '0' ? 'solid' : 'regular'} fa-star"></i>
        //                         <i class="fa-${item.review != '0' ? 'solid' : 'regular'} fa-star"></i>
        //                         <i class="fa-${item.review != '0' ? 'solid' : 'regular'} fa-star"></i>
        //                         <span>(${item.review} review)</span>
        //                     </div>
        //                     <h4>In hac habitasse platea dictumst tempor vet</h4>
        //                     <h3>${item.pricelt != 'none' ? '<span>$'+ item.pricelt +'</span>' : ''}$${item.price}</h3>
        //                 </div>
                        
        //             </div>
        //             </a>
                    
        //         </div>
        //             `
        //             count++;
        //             }
        //         })
        //         productrow.innerHTML = html;
        //         document.querySelector("#showing").innerHTML = `Showing ${count} results`
        //     })
        // })
        
    });

    let brands = document.querySelectorAll('.shopbrands .item label')

    brands.forEach(item =>{
        item.addEventListener('click',function(){
            if (shopbrands == item.innerHTML.replace(' SamSung', '')) {
                bottlesize = '';
                item.classList.remove('SamSunglactive');
            }else{
                bottlesize = item.innerHTML.replace(' cl', '');
                let a = document.querySelector('.SamSunglactive');
                if (a != null) {
                    a.classList.remove('SamSunglactive');
                }
                item.classList.add('SamSunglactive');
            }
        })
    })
    //const range = document.querySelector(".val");
    //range.addEventListener("change", () => {
    //    const min = document.querySelector("#min").value;
    //    const max = document.querySelector("#max").value;
    //    console.log("min" + min + "max" + max);
      
    //})

    // let productrow = document.querySelector('#products');
    // let productstring = '';

    function myFunction(){
        document.getElementById("myDropdown").classList.toggle("show");
    }

    let productsidebar = document.querySelector('#Main .left');
    let closeproductsidebar = document.querySelector('#closeproductsidebar');
    let openproductsidebar = document.querySelector('#openproductsidebar');

    closeproductsidebar?.addEventListener('click', function() {
        productsidebar.style.left = '-400px';
    })
    openproductsidebar?.addEventListener('click', function() {
        productsidebar.style.left = '0';
    })

})