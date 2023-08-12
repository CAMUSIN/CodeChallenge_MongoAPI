namespace ChallengeMongoAPI.Utilities
{
    public class Validators
    {
        public static bool LatLogValidator(float lat, float lon)
        {
            if ((lat > -89 && lat < 91) && (lon > -179 && lon < 181))
                return true;
            return false;
        }
    }
}
