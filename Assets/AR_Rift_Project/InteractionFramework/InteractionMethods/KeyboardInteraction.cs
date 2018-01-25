using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enums;

namespace FAR{

    public class KeyboardInteraction : InteractionMethod {
    	
    	string[] Keys = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","space","return","0","1","2","3","4","5","6","7","8","9"};
    

    	// Use this for initialization
    	override public void Start () {
    	
            base.Start();
    	
    	}

    	
    	
    	
    	// Update is called once per frame
	override public void Update()
    {
        base.Update();
    
    		if(Input.anyKey)
    		{
    		
    			foreach(string key in Keys)
    			{
    				if(Input.GetKey(key))
    				{
    					char[] temp = key.ToCharArray();
    					if(key=="return")
    					{
    						temp[0] = '#';
    					}
    					if(key=="space")
    					{
    						temp[0] = ' ';
    					}
    
    					if (Input.GetKey(KeyCode.LeftShift))
    					{
    						int t = (int) temp[0];
    						t -= 0x20;
    						temp[0] = (char)t;
    					}
                       
    					
    					KeyboardEvent evt = new KeyboardEvent(temp[0],this.gameObject.GetComponent<KeyboardInteraction>(),null);
    					fireEvent(evt);
    				}
    
    				
    				
    			}
    
    		}
            
            if(Input.anyKeyDown)
            {
            
				
                
                    if(Input.GetKeyDown(KeyCode.Backspace))
                    {
                        SingleKeyboardEvent evt = new SingleKeyboardEvent('*',this.gameObject.GetComponent<KeyboardInteraction>(),null);
                        fireEvent(evt);
                    }
                    
                foreach(string key in Keys)
                {
                    if(Input.GetKeyDown(key))
                    {
                        char[] temp = key.ToCharArray();
                        if(key=="return")
                        {
                            temp[0] = '#';
                        }
                        if(key=="space")
                        {
                            temp[0] = ' ';
                        }
                        
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            int t = (int) temp[0];
                            t -= 0x20;
                            temp[0] = (char)t;
                        }
                        
                        
                        SingleKeyboardEvent evt = new SingleKeyboardEvent(temp[0],this.gameObject.GetComponent<KeyboardInteraction>(),null);
                        fireEvent(evt);
                    }
                    
                    
                    
                }
                    
                    
                
                
            }
    		
    		
    
    	
    	}
    }

}
