using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
using UnityEngine.UI;
#endif
public class Options : MonoBehaviour
{
    public Button policyButton;
    public Button termsButton;
    public Button shareApp;

    [SerializeField] string _policyString;
    [SerializeField] string _termsString;

    private void Start()
    {
        policyButton.onClick.AddListener(PolicyView);
        termsButton.onClick.AddListener(TermsView);

        shareApp.onClick.AddListener(ShareApp);
    }

    void ShareApp()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }

    void PolicyView()
    {
        Application.OpenURL(_policyString);
    }
    void TermsView()
    {
        Application.OpenURL(_termsString);
    }
}
