using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	public bool OnlyDeactivate;

    public GameObject[] child;
    public int sortLayer = 0;
	void Start ()
	{
		if (particleSystem != null) 
		{
            Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
			//if(gameObject.name!="LEVEL_UP(Clone)")
			{
				particleSystem.renderer.sortingLayerName = "Default";
                particleSystem.renderer.sortingOrder = sortLayer;
                if (child != null)
                {
                    for (int i = 0; i < child.Length; i++)
                    {
                        if (child[i].particleSystem != null)
                        {
                            child[i].particleSystem.renderer.sortingLayerName = "Default";
                            child[i].particleSystem.renderer.sortingOrder = sortLayer;
                        }
                    }
                }
			}
		}
	}
	void OnEnable()
	{
		//particleSystem.renderer.sortingLayerName = "Foreground";
		StartCoroutine("CheckIfAlive");
	}
	
	IEnumerator CheckIfAlive ()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.5f);
			if(!particleSystem.IsAlive(true))
			{
				if(OnlyDeactivate)
				{
					#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
					#else
						this.gameObject.SetActive(false);
					#endif
				}
				else
					GameObject.Destroy(this.gameObject);
				break;
			}
		}
	}
}
