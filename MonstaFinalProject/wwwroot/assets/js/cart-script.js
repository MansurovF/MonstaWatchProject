// let search = document.querySelector('#SearchIcon');
// let srcdiv = document.querySelector('#Search-div');

// let sidebar = document.querySelector('#Sidebar');
// let sidebar_open = document.querySelector('#Sidebar-open');
// let sidebar_close = document.querySelector('#Sidebar-close');

// search.addEventListener("click", function() {
//     srcdiv.classList.toggle('active');
// })

// sidebar_open.addEventListener("click", function() {
//     sidebar.classList.add('active-sidebar');
// })

// sidebar_close.addEventListener("click", function() {
//     sidebar.classList.remove('active-sidebar');
// })




// function GetProducts(){
//     let basket=JSON.parse(localStorage.getItem('products'))
//     let count=0;
//     let total=document.querySelector('#shippings')
//     if (basket === null|| basket.length===0) {
//         document.querySelector("#show").classList.remove('d-none')
//     } else {
//         document.querySelector("#mainforcart").classList.remove('d-none')
//         let html=``
//         let totl = 0;
//         for (let product of basket) {
//         count+=Number(product.Price*product.Count)
//         html+=
//         `
//         <tbody>
//          <tr>
//              <td class="pro-thumbnail"><a href="#"><img
//                src="/assets/images/saat3.jpg" alt="Product"></a></td>
//              <td class="pro-title"><a href="#">Rinosin Glasses</a></td>
//              <td class="pro-price"><span>$395.00</span></td>
//              <td class="pro-quantity">
//                  <div class="pro-qty">
//                      <div class=" inputer count-input-block">
//                          <button type="button" class="minus"><i class="fa-solid fa-minus"></i></button>
//                          <input style="width: 50px; height: 37px; padding-left: 18px;" type="text" value="1" />
//                          <button type="button" class="plus"><i class="fa-solid fa-plus"></i></button>
//                      </div>
//                  </div>
//              </td>
//              <td class="pro-subtotal"><span>$395.00</span></td>
//              <td id="pro-remove"><i class="fa-regular fa-trash-can"></i></td>
//          </tr>
//      </tbody>
//         ` 
//         totl += Number(product.Price.slice(1))*Number(product.Count)
//     }
//     // document.getElementById('total').innerHTML = totl
//     total.innerHTML=`${totl} $`
//     console.log(totl);
//     document.querySelector('.mirta').innerHTML=html;
//     let lists=document.querySelectorAll('.lists')
//     for (let list of lists) {
//         let increase=list.querySelector('.plus')
//         let decrease=list.querySelector('.minus')
//         let del=list.querySelector('#pro-remove')
//         let input=list.querySelector('.inputer input')
//         increase.addEventListener('click',()=>{
//             let l_id=list.querySelector('.pro-subtotal').getAttribute('data-id')
//             let item=basket.find(item=>item.Id===l_id);
//             item.Count++
//             console.log(item.Count);
//             localStorage.setItem('products',JSON.stringify(basket))
//             location.reload()    
//         })
//         decrease.addEventListener('click',()=>{
//             let l_id=list.querySelector('#price .p_Id').getAttribute('data-id')
//             let item=basket.find(item=>item.Id===l_id);
//             if (item.Count>1) {
//                 item.Count--
//                 localStorage.setItem('products',JSON.stringify(basket))
//                 location.reload()
//             } else if(item.Count===1){
//                 basket.splice(basket.indexOf(item),1)
//                 localStorage.setItem('products',JSON.stringify(basket))
//                 location.reload()
//             }
           
//         })
//         del.addEventListener('click',()=>{
//             let l_id=list.querySelector('#price .p_Id').getAttribute('data-id')
//             let item=basket.find(item=>item.Id===l_id);
//             basket.splice(basket.indexOf(item),1)
//             localStorage.setItem('products',JSON.stringify(basket))
//             location.reload()
//         })
//         input.addEventListener('change',()=>{
//             let l_id=list.querySelector('#price .p_Id').getAttribute('data-id')
//             let item=basket.find(item=>item.Id===l_id);
//             item.Count=input.value
//             localStorage.setItem('products',JSON.stringify(basket))
//             location.reload()
//         })
//     }
//     }
// }  
// GetProducts();
// document.querySelector('#Clear .btn').addEventListener('click',()=>{
//     localStorage.removeItem('products')
//     location.reload()
// })