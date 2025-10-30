public class PolicyPopup : MonoBehaviour 
{
    public Text policyText;
    public Image background;
    public float displayDuration = 5f;
    
    private Coroutine hideCoroutine;
    
    public void ShowPolicy(string policyMessage) 
    {
        gameObject.SetActive(true);
        policyText.text = policyMessage;
        
        // Cancel previous hide coroutine if exists
        if (hideCoroutine != null) 
        {
            StopCoroutine(hideCoroutine);
        }
        
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }
    
    IEnumerator HideAfterDelay() 
    {
        yield return new WaitForSeconds(displayDuration);
        gameObject.SetActive(false);
    }
}