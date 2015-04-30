<?
//http://aegamemobile.com/MyAdsControl/index.php?GAME=BUBBLE_SHOOT_FREE&PLATFORM=IOS
	$game = $_GET['GAME'];
	$platform = $_GET['PLATFORM'];
	//echo $game;
	if($game =="BUBBLE_SHOOT_FREE")//link
	{	
		include_once "bubble_shoot_free.php";		
	}
	else if($game =="BAN_TRUNG_KHUNG_LONG_THUANVIETGAME")//link
	{
		include_once "bantrungkhunglong_thuanvietgame.php";
	}
	

?>