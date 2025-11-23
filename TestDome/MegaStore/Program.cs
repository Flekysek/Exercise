using System;

public class MegaStore
{
    public enum DiscountType
    {
        Standard,
        Seasonal,
        Weight
    }
  
    public static double GetDiscountedPrice(double cartWeight, 
        double totalPrice, 
        DiscountType discountType)
    {
        var finalPrice = 0.0;

        switch (discountType)
        {
            case DiscountType.Standard:
                finalPrice = totalPrice * (1.0 - 0.06);
                break;

            case DiscountType.Seasonal:
                finalPrice = totalPrice * (1.0 - 0.12);
                break;

            case DiscountType.Weight:
                if (cartWeight <= 10.0)
                {
                    finalPrice = totalPrice * (1.0 - 0.06);
                }
                else
                {
                    finalPrice = totalPrice * (1.0 - 0.18);
                }
                break;
                
            default:
                finalPrice = totalPrice;
                break;
        }

        return finalPrice;
    }
    
    public static void Main(string[] args)
    {
        Console.WriteLine(GetDiscountedPrice(12, 100, DiscountType.Weight));
    }
}