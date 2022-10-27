# JSFW.VS.Extensibility2019.CodeTemplate
코드 템플릿 관리 (( 💙 내가 가장 애정을 가진 프로그램 중 하나! 💙 ))

목적 : SI 다니면서 해당 프로젝트에서 계속 반복 사용되는 코드들을 별도로 관리하여 재사용하기 위해 만들었다. 
 이전엔 코드스니핏도 vs확장으로 만들어서 사용하고 있었으나... 이 프로그램 만든 후 계속 사용 중이다.

<br />
- 소스에서 컨텍스트 메뉴를 띄우면 [코드변환]<br />
![image](https://user-images.githubusercontent.com/116536524/198216118-36dcb37d-577e-43b1-b098-566cca823de8.png)<br />

- 코드변환 템플릿 창<br />
  : 등록해둔 코드목록이 보이는데 더블클릭하면 소스창에 바로 적용된다.<br />
![image](https://user-images.githubusercontent.com/116536524/198216535-4bbeeeaa-b5d7-4ab5-8945-f06310a48646.png)<br />

- 사용 예제<br />
![image](https://user-images.githubusercontent.com/116536524/198216858-948ecf7b-ac71-4816-9c98-6edaf0797b2c.png)<br />

- 코드 등록창<br />
![image](https://user-images.githubusercontent.com/116536524/198217251-768a42a7-3236-46ba-af00-a322ce5d6fac.png)<br />






--- 

소스내에서 텍스트박스 ( MIT )<br />
[ICSharpCode.AvalonEdit](https://github.com/icsharpcode/AvalonEdit) <br />


---
- package 폴더가 없어 확인해보니<br />
  Visual Studio로 소스 열릴때 알아서 생긴다. 
  그리고 관리자 모드가 아니면 아래와 같은 오류가 발생한다.

- 소스 열었을때 오류 발생시<br />
```diff
-오류		프로젝트에 "GenerateFileManifest" 대상이 없습니다.	JSFW.VS.Extensibility2019.VariableUsingView	
```
해결방법 :: 관리자 모드로 Visual Studio를 열면된다. 
<br /><br />


- 오류 2 <br />
심각도	코드	설명	프로젝트	파일	줄	비표시 오류(Suppression) 상태
오류		JSFW.VS.Extensibility2019.CodeTemplate: 'Microsoft.VSSDK.BuildTools' 패키지의 '15.6.152' 버전을 찾을 수 없습니다.
  C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\: 소스 'C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\'에서 패키지 'Microsoft.VSSDK.BuildTools.15.6.152'(을)를 찾을 수 없습니다.
  https://api.nuget.org/v3/index.json: 소스 'https://api.nuget.org/v3/index.json'에서 패키지 'Microsoft.VSSDK.BuildTools.15.6.152'(을)를 찾을 수 없습니다.
 프로젝트에 대한 NuGet 패키지 복원에 실패했습니다. 경고 및 오류에 대한 자세한 내용은 [오류 목록] 창을 참조하세요.				

 > package 폴더에 압축을 풀어서 해결. ( 관리자모드로도 안되네? )
[Microsoft.VSSDK.BuildTools.15.6.152.zip](https://github.com/aseuka/JSFW.VS.Extensibility2019.CodeTemplate/files/9876599/Microsoft.VSSDK.BuildTools.15.6.152.zip)

---
