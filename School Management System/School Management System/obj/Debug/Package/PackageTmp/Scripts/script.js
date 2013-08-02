
//NAVIGATION

$(function(){


    $('.midground').css({
        backgroundPosition: '0px 0px'
    });
    $('.foreground').css({
        backgroundPosition: '0px 0px'
    });
    
    var repeatM = 0;
    var repeatF = 0;
    var repeatB = 0;
    
    function midground(){
    
        if (repeatM == 0) {
            //alert(repeatM);
            repeatM = 1;
            //alert(repeatM);
            //alert(repeat);
            $('.midground').animate({
                backgroundPosition: "(0 -290000px)"
            }, 24000000, 'linear', function(){
                //alert('callback');
                repeatM = 0;
                midground();
                //alert('caller');
            });
        }
    }
    midground();
    
    function foreground(){
    
        if (repeatF == 0) {
            //alert(repeatM);
            repeatF = 1;
            //alert(repeatM);
            //alert(repeat);
            $('.foreground').animate({
                backgroundPosition: "(0 -290000px)"
            }, 16000000, 'linear', function(){
                //alert('callback');
                repeatF = 0;
                foreground();
                //alert('caller');
            });
        }
    }
    foreground();
	
});



//LOGO HOVER FADING EFFECT

$(function(){
    $("a.logo").hover(function(){
        $("span.logoHover").stop(true, true).animate({
            opacity: 'show'
        }, 500);
    }, function(){
        $("span.logoHover").animate({
            opacity: 'hide'
        }, 300);
    });
});

//SLIDESHOW 

$(function(){

    //$("#slideshowImages img:last").fadeIn(1000);
    $(window).bind('load', function(){
        $('#slideshowImages img:last').fadeIn();
    });
    
    $("ul#slideShowCats li").mouseover(function(){
        $(this).css("color", "#0c86b2");
    });
    
    function mouseOut(){
        $(this).css("color", "white");
    }
	
	var $cats = $("ul#slideShowCats li");
    
    $("ul#slideShowCats li").bind("mouseout", mouseOut);
    
	$("li.wd")
	.css("color","#0c86b2")
	.unbind("mouseout");
	
    $("ul#slideShowCats li").click(function(){
    
        $("ul#slideShowCats li").css("color", "white");
        
        $(this).css("color", "#0c86b2");
        
        var HoverPos = $(this).position().left;
        var HoverOff = ($(this).width() - $(".hoverShineNav").width()) / 2;
        var HoverMainPos = HoverPos + HoverOff;
        
        //alert(HoverMainPos);
        //alert(HoverOrig);
        
        $(".hoverShineNav").stop(true, true).animate({
            left: HoverMainPos
        }, 200);
        
        $(this).unbind("mouseout");
        $("ul#slideShowCats li").not(this).bind("mouseout", mouseOut);
        
        //Image Manipulation
        
        var imgSrc = $(this).attr("class");
        
        $("#slideshowImages img").hide();
        $("p.caption").hide();
        $("img." + imgSrc).fadeIn();
        
    });
    
});

//RECENT PROJECTS

$("document").ready(function(){

	var capHeight = 2 * $("p.projectCaption").height();
	//alert(capHeight);
	$("p.projectCaption").css("bottom", -capHeight);
		
	$(".projectImageHolder").hover(function(){
		
		$(this).find(".projectCaption").stop(true, true).animate({
			bottom: 0
		});
		
	},function(){
	
		$(this).find(".projectCaption").delay(500).animate({
			bottom: -capHeight
		});
	
	});
	
	
});

//BOTTOM LINKS 

$(function(){
	$("a.hire").hover(function(){
		$("a.hire .hover").stop(true, true).fadeIn();
	}, function(){
		$("a.hire .hover").fadeOut();
	});
	
	$("a.blaze").hover(function(){
		$("a.blaze .hover").stop(true, true).fadeIn();
	}, function(){
		$("a.blaze .hover").fadeOut();
	});
});

$(function(){


    $('.starsFront').css({
        backgroundPosition: '0px 0px'
    });
    $('.starsBack').css({
        backgroundPosition: '0px 0px'
    });
    
    function midgrounda(){
    
            $('.starsBack').animate({
                backgroundPosition: "(0 -290000px)"
            }, 20000000, 'linear', function(){
                //alert('callback');
                midgrounda();
                //alert('caller');
            });
        }
    midgrounda();
    
    function foregrounda(){
    
            $('.starsFront').animate({
                backgroundPosition: "(0 -290000px)"
            }, 10000000, 'linear', function(){
                //alert('callback');
                repeatF = 0;
                foregrounda();
                //alert('caller');
            });
        }
    foregrounda();
	
});

//FORM

$(function(){
	
	$("input, #message").focus(function(){
		$(".submitInfo")
		.html("Please click on the star below to submit the form.")
		.hide();
		
		var infoWidth = $(".submitInfo").width() / 2;
		
		$(".submitInfo")
		.css("marginLeft", -infoWidth)
		.fadeIn(1000);
		
		$(".submitInfo").stop(true,true).fadeIn(1000);
		
});
	
	$("input, #message").blur(function(){
		$(".submitInfo").stop(true,true).fadeOut(1000);
	});
	
	
	$("#star").mouseenter(function(){
		
		$("#blazing")
		.css({
			marginLeft: "0px"
		})
		.stop()
		.animate({
			marginLeft: "-200px"
		}, 800, "linear")
		
		$("#blazing2")
		.css({
			marginLeft: "0px"
		})
		.stop()
		.animate({
			marginLeft: "200px"
		}, 800, "linear");
		
	});
	
});

//MAIN FORM WORKING

$(function(){
	
	$("#star").click(function(){
		
	var name = $("input#name").val();
	var email = $("input#email").val();
	var subject = $("input#subject").val();
	var country = $("input#country").val();
	var phone = $("input#phone").val();
	var url = $("input#url").val();
	var message = $("textarea#message").val();
	
	var dataString = 'name='+ name + '&email=' + email + '&subject=' + subject + '&Country=' + country + '&Phone=' + phone + '&URL=' + url + '&Message=' + message;  
	//alert(dataString);
	
	if (
		$("#name").val() == "" || 
		$("#email").val() == "" || 
		$("#subject").val() == ""
		) 
	{
		$('.submitInfo').html("Please make sure that you filled all of the required fields correctly!").hide();
		
		var infoWidth = $(".submitInfo").width() / 2;
		$('.submitInfo').css("marginLeft", -infoWidth).fadeIn(1000);
		
		$(window).animate({top: "0"}, "fast");
		
	}
	
	else if (
		$("#message").val() == "" || $("#message").val().length <= 15
		) 
	{
		$('.submitInfo').html("Can't you say more than that?").hide();
		
		var infoWidth = $(".submitInfo").width() / 2;
		$('.submitInfo').css("marginLeft", -infoWidth).fadeIn(1000);
	}
	
	else {
		$.ajax({
			type: "POST",
			url: "php/process.php",
			data: dataString,
			success: function(){
				$('.submitInfo').html("Thanks for contacting us. We will get back to you soon!").hide();
				
				var infoWidth = $(".submitInfo").width() / 2;
				$('.submitInfo').css("marginLeft", -infoWidth).fadeIn(1000);
				
			}
		});
	}
	
	return false;	
	  
	});		  
});

