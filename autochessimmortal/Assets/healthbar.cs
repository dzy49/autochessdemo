using UnityEngine;
using System.Collections;
 
public class healthbar: MonoBehaviour {
	[SerializeField]
	//前方的图片  也就是红色图片
	private Transform front;
	//血量值   最大为1   用于调试设为public  正常时  需要改成private
	public float m_value;
	//血量属性
	public float Value
	{
		get{return m_value;}
		set{
			m_value=value;
			//血条两边都收缩
			front.localScale=new Vector3(m_value,1);
            
            //将血条向左移动
            front.localPosition=new Vector3(((1-m_value)*-1f)+0.08f,0.14f);
		}
	}
	void Update()
	{
        
        //实时监测血量
        Value = (float)(gameObject.transform.parent.GetComponent<Minons>().HP/100.0);
	}
}
