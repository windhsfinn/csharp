	public static class Cryptographie
	{
		public static string Crypt_RC4_Spritz(string sPlainText, string sKey, bool blSpritz = false, byte bytW = 1)
		{
			byte[] bytPlainArray = Encoding.UTF8.GetBytes(sPlainText);
			byte[] bytKeyArray = Encoding.UTF8.GetBytes(sKey);
			byte[] bytCipherArray;
			byte[] bytKeyStreamArray;
			string strFName4Hex;

			RC4_Spritz(bytPlainArray, bytKeyArray, out bytCipherArray, out bytKeyStreamArray, blSpritz, bytW);
			strFName4Hex = BitConverter.ToString(bytCipherArray).Replace("-", "");

			return strFName4Hex;
		}

		public static string DeCrypt_RC4_Spritz(string sHex, string sKey, bool blSpritz = false, byte bytW = 1)
		{
			byte[] bytPlainArray;
			byte[] bytCipherArray = Enumerable.Range(0, sHex.Length)
									  .Where(x => x % 2 == 0)
									  .Select(x => Convert.ToByte(sHex.Substring(x, 2), 16))
									  .ToArray();
			byte[] bytKeyArray = Encoding.UTF8.GetBytes(sKey);
			byte[] bytKeyStreamArray;
			string strFName4Plain;

			RC4_Spritz(bytCipherArray, bytKeyArray, out bytPlainArray, out bytKeyStreamArray, blSpritz, bytW);
			strFName4Plain = Encoding.UTF8.GetString(bytPlainArray);
			return strFName4Plain;
		}

		public static void RC4_Spritz(byte[] bytPlainArray, byte[] bytKeyArray, out byte[] bytCipherArray, out byte[] bytKeyStreamArray, bool blSpritz = false, byte bytW = 1)
		{
			int i, j, k, w, m, intKeyLen, intTemp, intPlainLen, z;
			int[] S = new int[256];
			intKeyLen = bytKeyArray.Length;
			intPlainLen = bytPlainArray.Length;
			bytCipherArray = new byte[intPlainLen];
			bytKeyStreamArray = new byte[intPlainLen];

			for (i = 0; i <= 255; i++)
			{
				S[i] = i;
			}

			j = 0;
			for (i = 0; i <= 255; i++)
			{
				j = (j + S[i] + bytKeyArray[i % intKeyLen]) % 256;
				intTemp = S[i];
				S[i] = S[j];
				S[j] = intTemp;
			}

			i = 0;
			j = 0;
			z = 0;
			for (m = 0; m <= intPlainLen - 1; m++)
			{
				if (blSpritz)
				{
					k = 0;
					w = bytW;
					i = (i + w) % 256;
					j = (k + S[(j + S[i]) % 256]) % 256;
					k = (i + k + S[j]) % 256;
					ArraySwitchElement(S, i, j); // switch element
					z = S[(j + S[(i + S[(z + k) % 256]) % 256]) % 256];
				}
				else
				{
					i = (i + 1) % 256;
					j = (j + S[i]) % 256;
					ArraySwitchElement(S, i, j); // switch element
					z = S[(S[i] + S[j]) % 256];
				}

				bytKeyStreamArray[m] = (byte)z;
				bytCipherArray[m] = (byte)(bytPlainArray[m] ^ z);
			}
		}
		private static void ArraySwitchElement(int[] array, int index1, int index2)
		{
			int temp = array[index1];
			array[index1] = array[index2];
			array[index2] = temp;
		}
	}
