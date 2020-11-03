namespace TakeOutFood
{
    using System;
    using System.Collections.Generic;

    public class App
    {
        private IItemRepository itemRepository;
        private ISalesPromotionRepository salesPromotionRepository;

        public App(IItemRepository itemRepository, ISalesPromotionRepository salesPromotionRepository)
        {
            this.itemRepository = itemRepository;
            this.salesPromotionRepository = salesPromotionRepository;
        }

        public string BestCharge(List<string> inputs)
        {
            List<Item> allItems = itemRepository.FindAll();
            List<SalesPromotion> allPromotions=salesPromotionRepository.FindAll();
            string purchaseMsg = "============= Order details =============\n";
            string promotionMsg = "-----------------------------------\n" +
                    "Promotion used:\n";
            double savedMoney = 0;
            double costMoney = 0;
            double finalCost = 0;
            string promotionItems = "";
            string output="";
            foreach (string a in inputs) 
            {
                var record = a.Split('x');
                var id = record[0].Trim();
                string name="";
                double number = double.Parse(record[1]);
                double price =0;
                bool isOnSale = IsOnSale(id);              
                foreach (Item b in allItems) 
                {
                    if (id == b.Id) 
                    {
                        name = b.Name;
                        price = b.Price;

                        if (isOnSale)
                        {
                            promotionItems += name;
                            savedMoney += b.Price * number * 0.5;
                        }
                        else 
                        {
                            promotionMsg = "";
                        }                      
                    }
                }
                var recordPrice = price * number;
                costMoney += recordPrice;
                purchaseMsg += name + " x " + number + " = " + recordPrice + "\n";
            }
            finalCost = costMoney - savedMoney;
            promotionMsg += promotionMsg + "(" + promotionItems + "), saving" + savedMoney + " yuan\n";
            output = purchaseMsg + promotionMsg;
            output += "Total: " + finalCost + "yuan\n"+ "===================================";           
            return output;
            Console.WriteLine(output);
        }

        public bool IsOnSale(string input)
        {
            List<SalesPromotion> allPromotions = salesPromotionRepository.FindAll();
            foreach (SalesPromotion a in allPromotions)
            {
                if (input == a.Type)
                {
                    return true;
                }              
            }
            return false;
        }
    }
}
