
namespace afternoon.Models
{
  public class Admission
  {
    public int Id { get; set; }
    public string Date { get; set; }
    public string BuyerId { get; set; }

    //this Profile property is only for populating
    public Profile Buyer { get; set; }
  }
}