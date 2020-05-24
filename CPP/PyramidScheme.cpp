#include <iostream>
#include <conio.h>
using namespace std;

//Compiler version g++ 6.3.0

int main()
{ 
    int N,i,j;
     cout<<"Enter value:\n ";
      cin>>N;
    for(i=1;i<=N;i++)
    {
      for(j=1;j<=i;j++)
      {
         cout<<j;
         cout<<" ";
      }
         cout<<"\n";
    }
