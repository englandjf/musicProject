using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class soundRecord : MonoBehaviour {

	int bufferSize;
	int numBuffers;
	int outputRate = 44100;
	string fileName = "recTest.wav";
	int headerSize = 44; //default for uncompressed wav

	bool recOutput;

	FileStream fileStream;

	void Awake()
	{
		AudioSettings.outputSampleRate = outputRate;
	}

	// Use this for initialization
	void Start () {
		AudioSettings.GetDSPBufferSize(out bufferSize,out numBuffers);
	}
	
	// Update is called once per frame
	void Update () {


		  
	
	}

	bool startRecording;

	public void start()
	{
		if (!recOutput) {
			print ("start");
			StartWriting (fileName); 
			recOutput = true;
		}
	}

	public void stop()
	{
		if (recOutput) {
			recOutput = false;
			WriteHeader ();      
			print ("rec stop");
		}
	}

	void StartWriting (string name)
	{
		fileStream = new FileStream(name, FileMode.Create); 
		byte emptybyte = new byte();

		for(int i = 0; i<headerSize; i++) //preparing the header
		{
			fileStream.WriteByte(emptybyte);
		}
	}

	void OnAudioFilterRead(float [] data,int channels)
	{
		
		if(recOutput)
		{
			ConvertAndWrite(data); //audio data is interlaced
		}
	}

	void ConvertAndWrite(float[] dataSource)
	{

		Int16[] intData = new Int16[dataSource.Length]; 
		//converting in 2 steps : float[] to Int16[], //then Int16[] to byte[]

		byte[] bytesData = new byte[dataSource.Length*2]; 
		//bytesData array is twice the size of 
		//dataSource array because a float converted in Int16 is 2 bytes.

		int rescaleFactor = 32767; //to convert float to Int16

		for (int i = 0; i<dataSource.Length;i++)
		{
			intData[i] = (short)(dataSource[i]*rescaleFactor);
			byte[] byteArr  = new byte[2];
			byteArr = BitConverter.GetBytes(intData[i]);
			byteArr.CopyTo(bytesData,i*2);
		}

		fileStream.Write(bytesData,0,bytesData.Length); 
	}
		
	void WriteHeader()
	{

		fileStream.Seek(0,SeekOrigin.Begin);

		byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
		fileStream.Write(riff,0,4);

		byte[] chunkSize = BitConverter.GetBytes(fileStream.Length-8);
		fileStream.Write(chunkSize,0,4);

		byte[] wave= System.Text.Encoding.UTF8.GetBytes("WAVE");
		fileStream.Write(wave,0,4);

		byte[]  fmt  = System.Text.Encoding.UTF8.GetBytes("fmt ");
		fileStream.Write(fmt,0,4);

		byte[]  subChunk1 = BitConverter.GetBytes(16);
		fileStream.Write(subChunk1,0,4);

		UInt16 two = 2;
		UInt16 one  = 1;

		byte[]  audioFormat  = BitConverter.GetBytes(one);
		fileStream.Write(audioFormat,0,2);

		byte[] numChannels  = BitConverter.GetBytes(two);
		fileStream.Write(numChannels,0,2);

		byte[]  sampleRate = BitConverter.GetBytes(outputRate);
		fileStream.Write(sampleRate,0,4);

		byte[] byteRate  = BitConverter.GetBytes(outputRate*4); 
		// sampleRate * bytesPerSample*number of channels, here 44100*2*2

		fileStream.Write(byteRate,0,4);

		UInt16 four = 4;
		byte[]  blockAlign = BitConverter.GetBytes(four);
		fileStream.Write(blockAlign,0,2);

		UInt16 sixteen  = 16;
		byte[] bitsPerSample  = BitConverter.GetBytes(sixteen);
		fileStream.Write(bitsPerSample,0,2);

		byte[] dataString  = System.Text.Encoding.UTF8.GetBytes("data");
		fileStream.Write(dataString,0,4);

		byte[] subChunk2 = BitConverter.GetBytes(fileStream.Length-headerSize);
		fileStream.Write(subChunk2,0,4);

		fileStream.Close();
	}


}
