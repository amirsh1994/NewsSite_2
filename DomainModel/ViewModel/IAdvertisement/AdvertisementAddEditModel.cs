namespace DomainModel.ViewModel.IAdvertisement;

public class AdvertisementAddEditModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string ImageUrl { get; set; }

    public string LinkUrl { get; set; }

    public string Alt { get; set; }

    public bool IsDefault { get; set; }
}