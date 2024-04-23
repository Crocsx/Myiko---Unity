[System.Serializable]
public class AuthRegistrationData
{
    public string email;
    public string password;
    public string displayName;

    public AuthRegistrationData(string email, string password, string displayName)
    {
        this.email = email;
        this.password = password;
        this.displayName = displayName;
    }
}