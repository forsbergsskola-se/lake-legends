using UI;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public QuitMenu quitMenu;
    private bool quitMenuIsOpen;
    private QuitMenu openMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !quitMenuIsOpen)
        {
            quitMenuIsOpen = true;
            openMenu = Instantiate(quitMenu, FindObjectOfType<Canvas>().transform);
            openMenu.SetUp(this);
            Time.timeScale = 0;
        }
        
        else if (Input.GetKeyDown(KeyCode.Escape) && quitMenuIsOpen)
        {
            Quit();
        }
    }
    
    public void Cancel()
    {
        quitMenuIsOpen = false;
        Destroy(openMenu.gameObject);
        Time.timeScale = 1;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
