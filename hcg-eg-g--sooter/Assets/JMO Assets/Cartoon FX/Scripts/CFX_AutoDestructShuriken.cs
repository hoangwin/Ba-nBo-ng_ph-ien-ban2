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
		if (GetComponent<ParticleSystem>() != null) 
		{
            Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
			//if(gameObject.name!="LEVEL_UP(Clone)")
			{
				GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Default";
                GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = sortLayer;
                if (child != null)
                {
                    for (int i = 0; i < child.Length; i++)
                    {
                        if (child[i].GetComponent<ParticleSystem>() != null)
                        {
                            child[i].GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Default";
                            child[i].GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = sortLayer;
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
			if(!GetComponent<ParticleSystem>().IsAlive(true))
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
