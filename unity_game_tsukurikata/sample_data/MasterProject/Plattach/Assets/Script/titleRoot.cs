﻿using UnityEngine;
using System.Collections;

public class titleRoot : MonoBehaviour {

	public Texture	title_texture = null;
	public Texture	start_texture = null;
	// ---------------------------------------------------------------- //
	public enum STEP {
		NONE = -1,
		WAIT_CLICK = 0,		// クリックまち.
		START_ACTION,		// クリックされたあとの演出.
		GAME_START,			// ゲームスタート.
		NUM,
	};
	public STEP			step      = STEP.NONE;
	public STEP			next_step = STEP.NONE;
	public float		step_timer = 0.0f;
	static private	float	START_ACTION_TIME = 1.0f;



	private SoundControl sound_control = null;


	// ================================================================ //
	// MonoBehaviour からの継承.
	void	Start()
	{
		this.next_step = STEP.WAIT_CLICK;

		this.sound_control = GameObject.Find("SoundRoot").GetComponent<SoundControl>();
	}
	
	void	Update()
	{
		// ---------------------------------------------------------------- //
		// ステップ内の経過時間を進める.
		this.step_timer += Time.deltaTime;
		// ---------------------------------------------------------------- //
		// 次の状態に移るかどうかを、チェックする.
		if(this.next_step == STEP.NONE) {
			switch(this.step) {
			case STEP.WAIT_CLICK:
			{
				if(Input.GetMouseButtonDown(0)) {
					this.next_step = STEP.START_ACTION;
				}
			}
				break;
				
			case STEP.START_ACTION:
			{
				if(this.step_timer > START_ACTION_TIME) {
					
					this.next_step = STEP.GAME_START;
				}
			}
				break;
			}
		}
		
		// ---------------------------------------------------------------- //
		// 状態が遷移したときの初期化.
		
		while(this.next_step != STEP.NONE) {
			this.step      = this.next_step;
			this.next_step = STEP.NONE;
			switch(this.step) {
			case STEP.START_ACTION:
				this.sound_control.playSound(Sound.SOUND.CLICK);
				break;
			case STEP.GAME_START:
			{
				Application.LoadLevel("GameScene");
				//Application.LoadLevel("resultScene");
			}
				break;
			}
			this.step_timer = 0.0f;
		}
		
		// ---------------------------------------------------------------- //
		// 各状態での実行処理.
		
		switch(this.step) {
			
		case STEP.WAIT_CLICK:
		{
		}
			break;
		}
	}
	
	void	OnGUI()
	{
		Rect	rect = new Rect();
		
		// 背景.
		rect.x = 0.0f;
		rect.y = 0.0f;
		// rect.width  = this.title_texture.width/2;
		// rect.height = this.title_texture.height/2;
		rect.width = Screen.width;
		rect.height = Screen.height;


		GUI.DrawTexture(rect, this.title_texture);
		
		// スタートボタン.
		float	scale = 1.0f;
		if(this.step == STEP.START_ACTION) {
			// クリックされると一瞬でかくなる（適当）.
			scale = this.step_timer/(START_ACTION_TIME/4.0f);
			scale = Mathf.Min(scale, 1.0f);
			scale = Mathf.Sin(scale*Mathf.PI);
			
			scale = Mathf.Lerp(1.0f, 1.2f, scale);
		}
		
		rect.width  = this.start_texture.width*scale;
		rect.height = this.start_texture.height*scale;
		
		rect.x = Screen.width*0.7f -rect.width/2.0f;
		rect.y = Screen.height*0.9f - rect.height/2.0f;
		
		GUI.DrawTexture(rect, this.start_texture);
	}
}
