<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="contact.aspx.cs" Inherits="WebTemplate.Contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    	<!--breadcrumbs-->
	<div class="breadcrumbs">
		<div class="container">
			<ol class="breadcrumb breadcrumb1 animated wow slideInLeft" data-wow-delay=".5s">
				<li><a href="index.html"><span class="glyphicon glyphicon-home" aria-hidden="true"></span>Anasayfa</a></li>
				<li class="active">Contact Us</li>
			</ol>
		</div>
	</div>
	<!--//breadcrumbs-->
	<!--contact-->
	<div class="contact">
		<div class="container">
			<div class="title-info wow fadeInUp animated" data-wow-delay=".5s">
				<h3 class="title">Kursat <span>Cakal</span></h3>
				<p> Her şey sizler için </p>
			</div>
			<div id="map" style="width:100%;height:500px"></div>
               <script src="semantic/packaged/javascript/semantic.min.js"></script>
              <script>
        function myMap() {
          var myCenter = new google.maps.LatLng(39.812513,32.835260);
          var mapCanvas = document.getElementById("map");
          var mapOptions = {center: myCenter, zoom: 20};
          var map = new google.maps.Map(mapCanvas, mapOptions);
          var marker = new google.maps.Marker({position:myCenter});
          marker.setMap(map);
          function flash() {
              $(".autumn.leaf")
             .transition('flash');
          }

         
         
        }
    </script>
         
 <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDAXTv3tUgQ8N3FYkblvmMv6YA3Cqtd5Qs&callback=myMap"></script>
	</div>
		</div>	

	<div class="address"><!--address-->
		<div class="container">
			<div class="address-row">
				<div class="col-md-6 address-left wow fadeInLeft animated" data-wow-delay=".5s">
					<div class="address-grid">
						<h4 class="wow fadeIndown animated" data-wow-delay=".5s">Bizimle iletisime gecin.</h4>
						<form>
                            <asp:TextBox ID="txtİsim" class="wow fadeIndown animated" data-wow-delay=".6s" type="text" placeholder="İsim" required="" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtEmail" class="wow fadeIndown animated" data-wow-delay=".7s" type="text" placeholder="Email" required="" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtKonu" class="wow fadeIndown animated" data-wow-delay=".8s" type="text" placeholder="Konu" required="" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtMesaj" class="wow fadeIndown animated" data-wow-delay=".8s" placeholder="Mesaj" required="" runat="server" TextMode="MultiLine"></asp:TextBox>
                            
                         
                            <asp:Button CssClass="wow fadeIndown animated ui"  data-wow-delay=".9s" type="submit" ID="btnGonder" runat="server" Text="Gönder" OnClick="btnGonder_Click" />
                            <%--<input class="wow fadeIndown animated ui" onclick="flash();" data-wow-delay=".9s" type="submit" value="Gonder">--%>
						</form>
					</div>
				</div>
				<div class="col-md-6 address-right">
					<div class="address-info wow fadeInRight animated" data-wow-delay=".5s">
						<h4>ADRES</h4>
						<p>Meşrutiyet Mahallesi Karanfil 2 Sokak No:50, 06420 Çankaya/Ankara</p>
					</div>
					<div class="address-info address-mdl wow fadeInRight animated" data-wow-delay=".7s">
						<h4>Telefon</h4>
						<p>+90 507 687 1477</p>
					</div>
					<div class="address-info wow fadeInRight animated" data-wow-delay=".6s">
						<h4>MAIL</h4>
						<p><a href="mailto:example@mail.com"> kursat.cakal@hotmail.com</a></p>
					</div>
				</div>
			</div>	
		</div>	
  		</div>	
	<!--//contact-->	

</asp:Content>
