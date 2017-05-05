<%@ Page Title="" Language="C#" MasterPageFile="~/NestedMasterPage1.master" AutoEventWireup="true" CodeBehind="products.aspx.cs" Inherits="WebTemplate.products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placeHolderbaslik" runat="server">
    	<!--breadcrumbs-->
	<div class="breadcrumbs">
		<div class="container">
			<ol class="breadcrumb breadcrumb1 animated wow slideInLeft" data-wow-delay=".5s">
				<li><a href="index.aspx"><span class="glyphicon glyphicon-home" aria-hidden="true"></span>Home</a></li>
				<li class="active">Products</li>
			</ol>
		</div>
	</div>
	<!--//breadcrumbs-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="product_icerik" runat="server">
  
      
    	<div class="col-md-9 product-model-sec">

            <asp:Repeater ID="rptProduct" runat="server">
                <ItemTemplate>
                       <div class="product-grids simpleCart_shelfItem wow flash" data-wow-delay=".5s">
					<div class="new-top">
						<%--<a href="single.html"><img style="height:145px;" src="<%#Eval("ImageURL")%>" class="img-responsive" alt=""/></a>--%>
                        <a href="single.html"><img style="height:145px;" src="<%#Eval("ImageURL").ToString()%>" class="img-responsive" alt=""/></a>
						<div class="new-text">
							<ul>
								<li><a href="single.aspx">Quick View</a></li>
								<li><input type="number" class="item_quantity" min="1" value="1"></li>
								<li><a class="item_add" href=""> Add to cart</a></li>
							</ul>
						</div>
					</div>
					<div class="new-bottom">
				<h5><a class="name" href="single.aspx"><%#Eval("ProductName").ToString().PadRight(10).Substring(0, 10).TrimEnd()%></a></h5>
						<div class="rating">
							<span class="on">☆</span>
							<span class="on">☆</span>
							<span class="on">☆</span>
							<span class="on">☆</span>
                          
							<span>☆</span>
                              <asp:LinkButton ID="LinkButton2" runat="server">☆</asp:LinkButton>
						</div>
						<div class="ofr">
							<p class="pric1"><del><%#Eval("UnitPrice","0:c")%></del></p>
							<p><span class="item_price"><%#Eval("UnitPrice","{0:c}")%></span></p>
						</div>
					</div>
				</div>
                </ItemTemplate>
            </asp:Repeater>
			   
	    </div>
</asp:Content>
