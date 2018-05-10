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
using InternationalOnlineShopping.Utility;
using Postal;
using InternationalOnlineShopping;
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

            if (Session["MemberId"] != null)
            {
                Cart c = new Cart();
                c.AddedOn = DateTime.Now;
                c.CartStatusId = 1;
                c.MemberId = memberId;
                c.ProductId = productId;
                c.UpdatedOn = DateTime.Now;
                unitOfWork.GetRepositoryInstance<Cart>().Add(c);
                //unitOfWork.SaveChanges();
                TempData["ProductAddedToCart"] = "Product added to cart successfully";
                return RedirectToAction("MyCart", "Shopping");

            }
            //else
            //{
            //    RedirectToAction("Index", "Register");
            //}
            return RedirectToAction("Index", "Register"); ;
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
        public Tuple<Boolean,String> PaymentCheck(ShippingDetail shippingDetail)
        {
            System.Diagnostics.Debug.WriteLine(shippingDetail.Card.CardNo);
     
            //string cardno = shippingDetail.Card.CardNo;
            string encryptedCardNumber = EncryptDecrypt.Encrypt(shippingDetail.Card.CardNo,true);
            string CCV = EncryptDecrypt.Encrypt(shippingDetail.Card.Ccv,true);


            decimal amountWithTax = shippingDetail.AmountPaid * (decimal)1.07;
         

            //   var existCard = unitOfWork.GetRepositoryInstance<Card>().GetFirstOrDefaultByParameter(i => 1 == 1);
            var existCard = unitOfWork.GetRepositoryInstance<Card>().GetFirstOrDefaultByParameter(i => i.CardNo == encryptedCardNumber && i.Ccv == CCV);
            if(existCard == null)               
            {
                return Tuple.Create(false,"Invalid Card Number !!");
            }
            else
            {
                int firstCardValue = Convert.ToInt32(existCard.CardValue);
                //card number match

                List<Transaction>transactions = unitOfWork.GetRepositoryInstance<Transaction>().GetAllRecordsIQueryable().Where(i => i.CardNo == encryptedCardNumber).ToList();
                if(transactions.Count == 0)
                {
                    if(shippingDetail.AmountPaid < existCard.CardValue)
                    {
                        //transaction approved for the first time 
                        Transaction transaction = new Transaction();
                        transaction.CardNo = encryptedCardNumber;
                        transaction.TxnAmount = Convert.ToInt32(amountWithTax);
                        transaction.Availability = Convert.ToInt32(firstCardValue - amountWithTax);
                        transaction.UsedAcount = Convert.ToInt32(shippingDetail.AmountPaid);
                        transaction.Key = true;
                        transaction.PayCard = true;
                        unitOfWork.GetRepositoryInstance<Transaction>().Add(transaction);

                        //unitOfWork.GetRepositoryInstance<ShippingDetail>().Add(shippingDetail);

                    }
                    else
                    {
                        //you dont have sufficient balance 
                       return Tuple.Create(false,"You dont' have sufficient amount for transaction !");
                    }
                }
                else
                {
                    //  Transaction transaction=
                    // if the transcation is already there where card no is same
                    Transaction transactionsLast = unitOfWork.GetRepositoryInstance<Transaction>().GetAllRecords().Where(i => i.CardNo == existCard.CardNo && i.Key == true).Last();

                    if(shippingDetail.AmountPaid < transactionsLast.Availability)
                    {
                        //transaction approved
                        Transaction transaction = new Transaction();
                        transaction.CardNo = encryptedCardNumber;

                        transaction.TxnAmount = Convert.ToInt32(amountWithTax);                       
                        transaction.Availability = Convert.ToInt32(transactionsLast.Availability - amountWithTax);
                        transaction.UsedAcount = transactionsLast.UsedAcount + Convert.ToInt32(amountWithTax);
                        transaction.Key = true;
                        transaction.PayCard = true;
                        unitOfWork.GetRepositoryInstance<Transaction>().Add(transaction);
                       // unitOfWork.GetRepositoryInstance<ShippingDetail>().Add(shippingDetail);

                    }
                    else
                    {
                        //you dont have sufficient balance 
                        return Tuple.Create(false,"You dont' have sufficient amount for transaction !");
                    }
                    

                }
                 return Tuple.Create(true,"");                
              }        
        }


        public ActionResult PaymentSuccess(ShippingDetail shippingDetail)
        {
            if(PaymentCheck(shippingDetail).Item1 == false)
            {
                ViewBag.ErrorMsg = PaymentCheck(shippingDetail).Item2;
                return View("ErrorTransaction");
            }
            else
            {
                ShippingDetail sd = new ShippingDetail();
                sd.MemberId = memberId;
                sd.AddressLine = shippingDetail.AddressLine;
                sd.City = shippingDetail.City;
                sd.State = shippingDetail.State;
                sd.Country = shippingDetail.Country;
                sd.ZipCode = shippingDetail.ZipCode;
                sd.OrderId = Guid.NewGuid().ToString();

                decimal amountWithTax = shippingDetail.AmountPaid * (decimal)1.07;
                sd.AmountPaid = amountWithTax;
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

                // SMTP sending email confirmation to customer
                Member member = unitOfWork.GetRepositoryInstance<Member>().GetFirstOrDefaultByParameter(i => i.MemberId == (int)Session["MemberId"]);
                
                dynamic email = new Email("Success");
                email.To = member.EmailId;
                email.Name = member.FirstName;
                email.Send();
                return View(sd);
            }
         

        }
        
    }
}