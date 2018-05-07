using InternationalOnlineShopping.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternationalOnlineShopping.Models;
using System.Data.SqlClient;
using System.Data;
using InternationalOnlineShopping.Filters;


namespace OnlineShopping.Controllers
{
    [FrontPageActionFilter]
    //[AuthorizeUser]
    public class ShoppingController: Controller
    {
        #region Other Class references ...         
        // Instance on Unit of Work         
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        private int _memberId;
        public int memberId
        {
            get { return Convert.ToInt32(Session["MemberId"]); }
            set { _memberId = Convert.ToInt32(Session["MemberId"]); }
        }
        #endregion

        /// <summary>
        /// Add Product To Cart
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult AddProductToCart(int productId)
        {
            Cart c = new Cart();
            c.AddedOn = DateTime.Now;
            c.CartStatusId = 1;
            c.MemberId = memberId;
            c.ProductId = productId;
            c.UpdatedOn = DateTime.Now;
            unitOfWork.GetRepositoryInstance<Cart>().Add(c);
            unitOfWork.SaveChanges();
            TempData["ProductAddedToCart"] = "Product added to cart successfully";
            return RedirectToAction("MyCart","Shopping");
        }

        /// <summary>
        /// MyCart
        /// </summary>
        /// <returns>List of cart items</returns>
        public ActionResult MyCart()
        {
            List<USP_MemberShoppingCartDetails_Result> cd = unitOfWork.GetRepositoryInstance<USP_MemberShoppingCartDetails_Result>().GetResultBySqlProcedure("USP_MemberShoppingCartDetails @memberId",
                new SqlParameter("memberId",System.Data.SqlDbType.Int) { Value = memberId }).ToList();
            return View(cd);
        }

        /// <summary>
        /// Remove Cart Item
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult RemoveCartItem(int productId)
        {
            Cart c = unitOfWork.GetRepositoryInstance<Cart>().GetFirstOrDefaultByParameter(i => i.ProductId == productId && i.MemberId == memberId && i.CartStatusId == 1);
            c.CartStatusId = 2;
            c.UpdatedOn = DateTime.Now;
            unitOfWork.GetRepositoryInstance<Cart>().Update(c);
            unitOfWork.SaveChanges();
            return RedirectToAction("MyCart");
        }

        /// <summary>
        /// CheckOut the Cart items
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckOut()
        {
            List<USP_MemberShoppingCartDetails_Result> cd = unitOfWork.GetRepositoryInstance<USP_MemberShoppingCartDetails_Result>().GetResultBySqlProcedure("USP_MemberShoppingCartDetails @memberId",
               new SqlParameter("memberId",System.Data.SqlDbType.Int) { Value = memberId }).ToList();
            ViewBag.TotalPrice = cd.Sum(i => i.Price);
            ViewBag.CartIds = string.Join(",",cd.Select(i => i.CartId).ToList());
            return View(cd);
        }

        /// <summary>
        /// Payment Success
        /// </summary>
        /// <param name="shippingDetails"></param>
        /// <returns></returns>
        ///   
        /*
        public ActionResult PaymentSuccess(ShippingDetail shippingDetail)
        {
          
            ShippingDetail sd = new ShippingDetail();
            sd.MemberId = memberId;
            sd.AddressLine = shippingDetail.Address;
            sd.City = shippingDetail.City;
            sd.State = shippingDetail.State;
            sd.Country = shippingDetail.Country;
            sd.ZipCode = shippingDetail.ZipCode;
            sd.OrderId = Guid.NewGuid().ToString();
            sd.AmountPaid = shippingDetail.TotalPrice;
            sd.PaymentType = shippingDetail.PaymentType;
            unitOfWork.GetRepositoryInstance<ShippingDetail>().Add(sd);
            unitOfWork.GetRepositoryInstance<Cart>().UpdateByWhereClause(i => i.MemberId == memberId && i.CartStatusId == 1,(j => j.CartStatusId = 3));
            unitOfWork.SaveChanges();
            if(!string.IsNullOrEmpty(Request["CartIds"]))
            {
                int[] cartIdsToUpdate = Request["CartIds"].Split(',').Select(Int32.Parse).ToArray();
                unitOfWork.GetRepositoryInstance<Cart>().UpdateByWhereClause(i => cartIdsToUpdate.Contains(i.CartId),(j => j.ShippingDetailId = sd.ShippingDetailId));
                unitOfWork.SaveChanges();

            }
            
            return View(sd);
        }
        */
    }
}