using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageParser : MonoBehaviour 
{
	public LevelData LoadData(TextAsset File) {
		
		//the object to return. Look at LevelData.cs for all the variables it can hold
		LevelData retVal = new LevelData();

		//split the file into sections, and run through all of them
		string[] Sections = File.text.Trim().Split('(');
		foreach (string i in Sections) {

			//split the name off from the rest of it. The if statement makes sure that there's a closing character
			string[] i2 = i.Trim().Split(')');
			if (i2.Length > 1) {

				//check name of section
				switch (i2[0].Trim().ToUpper()) {
					case "DATA":
						//function names
						string[] i3 = i2[1].Trim().Split('{');
						foreach (string i4 in i3) {

							//split name off and check for closing character
							string[] i5 = i4.Trim().Split('}');
							if (i5.Length > 1) {
							
								//check name of function
								switch (i5[0].Trim().ToUpper()) {
									case "NAME":
										
										//split variable/s off, making sure they exist
										string[] i6a = i5[1].Trim().Split('[');
										if (i6a.Length > 1) {

											//split variable/s off and check for closing character
											string[] i7 = i6a[1].Trim().Split(']');
											if (i7.Length > 1) {

												//set name
												retVal.LevelName = i7[0];
											}
										}
										break;
									case "DESCRIPTION":

										//split variable/s off, making sure they exist
										string[] i6b = i5[1].Trim().Split('[');
										if (i6b.Length > 1) {

											//split variable/s off and check for closing character
											string[] i7 = i6b[1].Trim().Split(']');
											if (i7.Length > 1) {

												//set name
												retVal.LevelName = i7[0];
											}
										}
										break;
									case "MUSIC":

										//split variable/s off, making sure they exist
										string[] i6c = i5[1].Trim().Split('[');
										if (i6c.Length > 1) {

											//split variable/s off and check for closing character
											string[] i7 = i6c[1].Trim().Split(']');
											if (i7.Length > 1) {
												//set music array
												retVal.LevelMusic = i7[0].Trim().Split(',');
											}
										}
										break;
									case "ICON":

										//split variable/s off, making sure they exist
										string[] i6d = i5[1].Trim().Split('[');
										if (i6d.Length > 1) {

											//split variable/s off and check for closing character
											string[] i7 = i6d[1].Trim().Split(']');
											if (i7.Length > 1) {
												//set name
												retVal.Icon = i7[0];
											}
										}
										break;
								}
							}
						}
						break;
					case "SCRIPTS":
						//script package names
						string[] i3b = i2[1].Split('{');
						foreach (string i4 in i3b) {
							//split name off and check for closing character
							string[] i5 = i4.Split('}');
							if (i5.Length > 1) {
								//split script names off, making sure they exist
								string[] i6 = i5[1].Trim().Split('[');
								if (i6.Length > 1) {
									//split script names off and check for closing character
									string[] i7 = i6[1].Split(']')[0].Trim().Split(',');
									if (i7.Length > 1) {
										string[] i8 = new string[i7.Length+1];
										i8[0] = i5[0].Trim(); ;
										i7.CopyTo(i8, 1);
										//add to the script package list
										retVal.Scripts.Add(i8);
									}
								}
							}
						}
						break;
					case "VISUAL":
						//internal file names
						string[] i3c = i2[1].Split('{');
						foreach (string i4 in i3c) {
							//split name off and check for closing character
							string[] i5 = i4.Split('}');
							if (i5.Length > 1) {
								//split file name off, making sure it exists
								string[] i6 = i5[1].Split('[');
								if (i6.Length > 1) {
									//split file name off and check for closing character
									string[] i7 = i6[1].Trim().Split(']');
									if (i7.Length > 1) {
										string[] i8 = new string[] { i5[0].Trim(), i7[0] };
										//add to the visual list
										retVal.Scripts.Add(i8);
									}
								}
							}
						}
						break;
					case "OBJECTS":
						//script packages on object
						string[] i3d = i2[1].Split('{');
						foreach (string i4 in i3d) {
							//split name off and check for closing character
							string[] i5 = i4.Split('}');
							if (i5.Length > 1) {
								StageObject temp = new StageObject();
								string[] i6 = i5[0].Split(',');
								temp.scripts = new string[i6.Length];
								for (int i7 = 0; i7 < i6.Length; i7++) {
									temp.scripts[i7] = i6[i7].Trim();
								}
								//split file name off, making sure it exists
								string[] i8 = i5[1].Split('[');
								if (i8.Length > 1) {
									//split file name off and check for closing character
									string[] i9 = i8[1].Split(']');
									if (i9.Length > 1) {
										string[] i10 = i9[0].Split(';');
										foreach (string i11 in i10) {
											string[] i12 = i11.Split(':')[1].Split(',');
											switch (i11.Split(':')[0].Trim().ToUpper()) {
												//
												//PLACE TO ADD MORE FEATURES TO OBJECT LOADING
												//
												case "XY":
													if (i12.Length >= 2) {
														temp.pos = new Vector3(float.Parse(i12[0]), float.Parse(i12[1]), temp.pos.z);
													}
													break;
												case "TLBR":
													if (i12.Length >= 4) {
														temp.pos = new Vector3(float.Parse(i12[0]), float.Parse(i12[1]), temp.pos.z);
														temp.scale = new Vector3(float.Parse(i12[2]) - float.Parse(i12[0]), float.Parse(i12[3]) - float.Parse(i12[1]), temp.pos.z);
													}
													break;
												case "TLCBRF":
													if (i12.Length >= 6) {
														temp.pos = new Vector3(float.Parse(i12[0]), float.Parse(i12[1]), float.Parse(i12[2]));
														temp.scale = new Vector3(float.Parse(i12[3]) - float.Parse(i12[0]), float.Parse(i12[4]) - float.Parse(i12[1]), float.Parse(i12[5]) - float.Parse(i12[2]));
													}
													break;
												case "LINE2D":
													if (i12.Length >= 2) {
														for (int i13 = 0; i13 < i12.Length - 1; i13 += 2) {
															temp.lineCol.Add(new Vector3(float.Parse(i12[i13]), float.Parse(i12[i13+1])));
														}
													}
													break;
											}
										}
										retVal.Objects.Add(temp);
									}
								}
							}
						}
						break;
				}
			}
		}
		return retVal;
	}
}
