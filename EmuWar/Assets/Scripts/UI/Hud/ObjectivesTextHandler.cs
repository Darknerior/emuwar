using System;
using System.Linq;
using TMPro;
using UnityEngine;
[AddComponentMenu(" Game Scripts / UI / HUD / ObjectivesTextHandler",2)]
public class ObjectivesTextHandler : MonoBehaviour
{
  private TextMeshProUGUI objectiveBanner;
  private TextMeshProUGUI progressMarker;
  private void Start()
  {
      var texts = GetComponentsInChildren<TextMeshProUGUI>().ToList();
      var item1 = texts.First(x => x.gameObject.name == "Objectives");
      objectiveBanner = item1;
      texts.Remove(item1);
      progressMarker = texts.First();
      DisableText();
  }
  

  public ObjectivesTextHandler EnableObjectiveBannerWithString(string line)
  {
      objectiveBanner.text = line;
      objectiveBanner.gameObject.SetActive(true);
      return this;
  }

  public ObjectivesTextHandler EnableProgressMarkerWithString(string line)
  {
      progressMarker.text = line;
      progressMarker.gameObject.SetActive(true);
      return this;
  }

  public ObjectivesTextHandler DisableText(bool objectives = true, bool progress = true)
  {
      objectiveBanner.gameObject.SetActive(!objectives);
      progressMarker.gameObject.SetActive(!progress);
      return this;
  }

  public ObjectivesTextHandler UpdateProgressText(string line)
  {
      progressMarker.text = line;
      return this;
  }
}
