1. Programi ayaga kaldirmak icin ilk adim Sql Server da yeni bir Database olusturmak.
Ben sql server kullandim, database adi da BuyWithOffer, mumkunse bu isimle olusturulabilir. Farkli bir isimle olusuturulmasi durumunda 
SqlScripts.txt dosyasinda use [database] scripti guncellenmelidir.

2. adim nuget-package manager console dan Update-Database komutunu kullanip tablolari database e kopyalamak.

3. adimda mock data kullanmak istenirse SqlScripts.txt dosyasi icindeki scriptler kullanilabilir. Ozellikle color brand ve usage status icin 
kullanilmali, kategory category controllerdan, urunler ve teklifler de yine controller lar araciligi ile olusturulabilir.

4.Adim register veya login olmak, scriptlerde 3 tane user insert scripti var, kullanilmak istenirse user sifreleri ayni "Asd123.:."
Sisteme giris yaptiktan sonra metodlar kullanima acik olucaktir. Registerda microsoft identity kullandigim icin register i takiben sisteme 
otomatik olarak giris yapmis oluyoruz. Token bearer olarak header a eklemeye gerek kalmiyor.

5.Artik proje ayaga kalkmistir ve methodlar test edilebilir.

6. Bir urune teklif yapildiktan sonra teklifi sadece urunu yayinlayan kullanici onaylayabilir.
 Ve teklifi sadece o teklifi veren kullanici satin alabilir.
 Bu yuzden teklif verirken ve urunu satin alirken aktif kullaniciyi degistirmek gerekecektir
 Mock datalarda ilk 10 urun icin teklifler olusturulmustur
  Metodlari denerken mock userlar kullanilmak istenirse:
--Urunleri yayinlayan kullanici			: "emre.678@hotmail.com" Sifresi: "Asd123.:."
--Teklifleri veren kullanici 			: "emre.678@outlook.com" Sifresi: "Asd123.:."

--Urunu ve teklifi olmayan kullanici    : "emregocmen48@gmail.com" Sifresi: "Asd123.:."

Urunler yayinlandiginda isOfferable degeri false geldigi icin ProductController OpenToOffer metodu urunleri teklife acar.
OfferController MakeOffer methodu ile verilen teklif, urunu yayinlayan kullanici tarafindan ProductController ConfirmOffer methodu ile 
onaylandiktan sonra, OfferController BuyOffer metodu ile teklifi veren kullanici tarafindan satin alinabilir

Aciklama gerektiren yerlerde kod icinde yorum satirlari ile aciklama yapmaya calistim.

Devam eden maddelerde kendimce gerekli gordugum projeye ekstra olarak eklenen metodlar aciklanmistir.

7.Onaylanan teklifler sale tablosuna kaydedilir. SaleController GetAll metodu satislari goruntulemek icin eklenmistir.

8.Kullanici kayit oldugunda, hatali sifreden dolayi hesabi bloke oldugunda, urunu satildiginda ve verdigi teklif 
kendi kontrolu disindaki bir sebepten(ornegin urun satilmistir) iptal oldugunda kullanicilara mail gonderilir. Mailler Email tablosuna kaydedilir 
ve o tablodan gerekli mail cekilip gonderilir. 5 kez denenip basarisiz olan mailler failed olarak guncellenir. SendFailedMails methodu bu mailleri
gondermeyi tekrar denemek icin eklenmistir.

9.ProductController a GetByUser metodu aktif kullanicinin kendine ait urunleri listeleyebilmesi icin eklenmistir.

10. ProductController da GetAll metodu satilmis olan ve guncel olarak satisa acik olmayan urunleri de listeledigi icin urun sayfasinda urunler 
GetOpenForSale metodu ile listelenmelidir.

