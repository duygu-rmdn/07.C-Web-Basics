namespace SharedTrip.Servises
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
