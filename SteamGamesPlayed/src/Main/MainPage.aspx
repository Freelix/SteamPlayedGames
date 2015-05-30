<%@ Page Title="Home Page" Language="C#" MasterPageFile="Site.master" AutoEventWireup="true"
    CodeBehind="MainPage.aspx.cs" Inherits="SteamGamesPlayed.MainPage" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<div id="templatemo_content_wrapper">
    <div id="templatemo_content">
        <form id="mainForm" runat="server">
            <asp:Button ID="BtnOrderByAlpha" CssClass="default_btn" runat="server" Text="Alphabetical Order" />
            <asp:Button ID="BtnOrderByCompleted" CssClass="default_btn" runat="server" Text="Completed/Incompleted" />
            <asp:Button ID="BtnOrderByTimePlayed" CssClass="default_btn" runat="server" Text="Order by time played" />
            
            <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManagerScores" runat="server"/>

            <!-- Makes the button BtnRetrieveScores clickable without refreshing the page -->
            <div class="updatePanelDiv">
                <asp:UpdatePanel ID="UpdatePanelScores" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="BtnRetrieveScores" CssClass="default_btn" runat="server" Text="Retrieve Metacritic's scores" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnRetrieveScores" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

                <!-- Makes the label ProgressLabel refreshing without reloading the page -->
                <asp:Timer ID="TimerScoreUpdate" runat="server" Interval="500" OnTick="TimerScoreUpdate_Tick" Enabled="false"/> 
                <asp:UpdatePanel ID="UpdatePanelScoresLabel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="ProgressLabel" CssClass="progressLabel" Text="" Visible="false"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="TimerScoreUpdate" EventName="Tick" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </form>

        <asp:Panel ID="PageColumns" runat="server"/>
    
    	<div class="cleaner"></div>
	</div> <!-- end of content -->
</div> <!-- end of content_wrapper -->
</asp:Content>


