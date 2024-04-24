[System.Serializable]
public class AuthRegistrationDto
{
    public string email;
    public string password;
    public string displayName;

    public AuthRegistrationDto(string email, string password, string displayName)
    {
        this.email = email;
        this.password = password;
        this.displayName = displayName;
    }
}