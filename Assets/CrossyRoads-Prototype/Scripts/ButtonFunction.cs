using UnityEngine;

	public class ButtonFunction : MonoBehaviour
	{
		public Transform functionTarget;
		public string functionName;
		public string functionParameter;

		void OnMouseOver()
		{
			if ( Time.deltaTime > 0 && Input.GetMouseButton(0) )    ExecuteFunction();
		}
		void ExecuteFunction()
		{
			if( functionName != string.Empty )
			{  
				if( functionTarget )
				{
					functionTarget.SendMessage(functionName, functionParameter);
				}
			}
		}
	}
