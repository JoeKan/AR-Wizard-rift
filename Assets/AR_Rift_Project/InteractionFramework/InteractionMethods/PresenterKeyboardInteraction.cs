using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enums;

namespace FAR{

    public class PresenterKeyboardInteraction : InteractionMethod {

        KeyCode[] Keys = {KeyCode.Mouse0, KeyCode.Mouse1,KeyCode.A, KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,
                          KeyCode.AltGr, KeyCode.Ampersand, KeyCode.Asterisk, KeyCode.At, KeyCode.B, KeyCode.BackQuote, KeyCode.Backslash, KeyCode.Backspace, KeyCode.Break, KeyCode.C, KeyCode.CapsLock,
                          KeyCode.Caret, KeyCode.Clear, KeyCode.Colon, KeyCode.Comma, KeyCode.D, KeyCode.Delete, KeyCode.Dollar, KeyCode.DoubleQuote, KeyCode.DownArrow, KeyCode.E, KeyCode.End,
                          KeyCode.Equals, KeyCode.Escape, KeyCode.Exclaim, KeyCode.F, KeyCode.F1, KeyCode.F10, KeyCode.F11, KeyCode.F12, KeyCode.F13, KeyCode.F14, KeyCode.F15, KeyCode.F2, KeyCode.F3,
                          KeyCode.F4, KeyCode.F5, KeyCode.F6, KeyCode.F7, KeyCode.F8, KeyCode.F9, KeyCode.G, KeyCode.Greater, KeyCode.H, KeyCode.Hash, KeyCode.Help, KeyCode.Home, KeyCode.I,
                          KeyCode.Insert, KeyCode.J, KeyCode.K, KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7,
                          KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.KeypadDivide, KeyCode.KeypadEnter, KeyCode.KeypadEquals, KeyCode.KeypadMinus, KeyCode.KeypadMultiply, KeyCode.KeypadPeriod,
                          KeyCode.KeypadPlus, KeyCode.L, KeyCode.LeftAlt, KeyCode.LeftArrow, KeyCode.LeftBracket, KeyCode.LeftCommand, KeyCode.LeftControl, KeyCode.LeftParen, KeyCode.LeftShift, KeyCode.LeftWindows,
                          KeyCode.Less, KeyCode.M, KeyCode.Menu, KeyCode.Minus, KeyCode.N, KeyCode.Numlock, KeyCode.O, KeyCode.P, KeyCode.PageDown, KeyCode.PageUp, KeyCode.Pause, KeyCode.Period, KeyCode.Plus,
                          KeyCode.Print, KeyCode.Q, KeyCode.Question, KeyCode.Quote, KeyCode.R, KeyCode.Return, KeyCode.RightAlt, KeyCode.RightArrow, KeyCode.RightBracket, KeyCode.RightCommand, KeyCode.RightControl,
                          KeyCode.RightParen, KeyCode.RightShift, KeyCode.RightWindows, KeyCode.S, KeyCode.ScrollLock, KeyCode.Semicolon, KeyCode.Slash, KeyCode.Space, KeyCode.T, KeyCode.Tab, KeyCode.U,
                          KeyCode.Underscore, KeyCode.UpArrow, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z};
    
    	override public void Start ()
        {    	
            base.Start();
    	}    	
    	
		override public void Update()
		{
			base.Update();

            if (Input.anyKey)
            {
                foreach (KeyCode key in Keys)
                {
                    if (Input.GetKey(key))
                    {
                        PresenterKeyboardEvent evt = new PresenterKeyboardEvent(key, this.gameObject.GetComponent<KeyboardInteraction>(), null);
                        fireEvent(evt);
                    }
                }
            }
            
            if(Input.anyKeyDown)
            {                           
                foreach(KeyCode key in Keys)
                {
                    if(Input.GetKeyDown(key))
                    {
                        SinglePresenterKeyboardEvent evt = new SinglePresenterKeyboardEvent(key,this.gameObject.GetComponent<KeyboardInteraction>(), null);
                        fireEvent(evt);
                    }
                }
            }
    	}
    }
}