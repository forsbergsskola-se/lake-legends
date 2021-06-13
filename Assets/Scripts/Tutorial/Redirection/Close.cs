using UnityEngine;

namespace Tutorial.Redirection
{
    [CreateAssetMenu(menuName = "ScriptableObjects/RedirectionDefinitionClose")]
    public class Close : RedirectionDefinition
    {
        public override void Redirect(TutorialPopup tutorialPopup)
        {
            tutorialPopup.UnPause();
            Destroy(tutorialPopup.gameObject);
        }
    }
}