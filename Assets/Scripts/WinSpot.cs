using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSpot : Obstacle {

	public List<Collectible> starList;
	public int minWinStars;
	public int displayStars; //for debug only, get rid of this later

	bool checkForWin = false;

	public override void HitByLight(Vector3 lightOrigin, Vector3 lightEnd)
	{
		checkForWin = true;
	}

	void LateUpdate ()
	{
		if (checkForWin) {
			checkForWin = false;
			int litStars = 0;
			for (int i = 0; i < starList.Count; i++) {
				if (starList [i].shineOn) {
					litStars++;
				}
			}

			if (litStars >= minWinStars) {
				// you beat the round! advance to the next stage! ...or something
				print ("VICTORY!");
			}

			displayStars = litStars;
		}
	}
}
