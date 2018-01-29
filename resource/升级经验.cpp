#include <iostream>
using namespace std;

float score(int gs, int js, float gr, float jr){
	return 690*gr*gs+60+15771*gr*jr*js;
}

int main(){
	int gs=100,js=50;
	float gr=0.01,jr=0.02;
	float lastScore=0;
	float newScore=0;
	for(int i=0;i<11;i++){
		if(i==9)
			continue;
		newScore=score(gs+10*i,js+i,gr+0.0005*i,jr+0.0005*i);
		cout<<newScore<<" "<<newScore-lastScore<<endl;
		lastScore=newScore;
	}
	return 0;
}
