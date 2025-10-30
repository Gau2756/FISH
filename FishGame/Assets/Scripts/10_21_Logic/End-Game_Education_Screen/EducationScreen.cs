public class EducationScreen : MonoBehaviour 
{
    public Text factsText;
    
    public void ShowFacts() 
    {
        string facts = 
            "DID YOU KNOW?\n\n" +
            "• Pacific salmon populations have declined 40% since 1980\n" +
            "• Over 1,000 dams block historic salmon migration routes\n" +
            "• Salmon are critical to ecosystem health - their bodies provide nutrients to forests\n" +
            "• Indigenous communities depend on salmon for food sovereignty\n" +
            "• Dam removal projects have successfully restored salmon populations\n\n" +
            "What YOU can do:\n" +
            "• Support river restoration organizations\n" +
            "• Contact representatives about environmental protections\n" +
            "• Choose sustainable seafood\n" +
            "• Learn about local watershed health";
        
        factsText.text = facts;
    }
}