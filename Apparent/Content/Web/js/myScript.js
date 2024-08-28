
// =======================================================================
	// main nav js start
// =======================================================================
// Used jquery v3.5.1 and font-awesome v4.7.0
    $(document).ready(function () {
        $(".nav .dropdown-menu")
            .prev("a")
            .on("click", function (e) {
                e.preventDefault();
                $(this).parent().find(".dropdown-menu").slideToggle();
            });
    });



// =======================================================================
    // software tab to accordion
// =======================================================================
 // tabbed content
    // http://www.entheosweb.com/tutorials/css/tabs.asp
    $(".tab_content").hide();
    $(".tab_content:first").show();

  /* if in tab mode */
    $("ul.tabs li").click(function() {
        
      $(".tab_content").hide();
      var activeTab = $(this).attr("rel"); 
      $("#"+activeTab).fadeIn();        
        
      $("ul.tabs li").removeClass("active");
      $(this).addClass("active");

      $(".tab_drawer_heading").removeClass("d_active");
      $(".tab_drawer_heading[rel^='"+activeTab+"']").addClass("d_active");
      
    });
    /* if in drawer mode */
    $(".tab_drawer_heading").click(function() {
      
      $(".tab_content").hide();
      var d_activeTab = $(this).attr("rel"); 
      $("#"+d_activeTab).fadeIn();
      
      $(".tab_drawer_heading").removeClass("d_active");
      $(this).addClass("d_active");
      
      $("ul.tabs li").removeClass("active");
      $("ul.tabs li[rel^='"+d_activeTab+"']").addClass("active");
    });
    
    
    /* Extra class "tab_last" 
       to add border to right side
       of last tab */
    $('ul.tabs li').last().addClass("tab_last");
    

// =======================================================================
    // show more
// =======================================================================
$('#show-more-content').hide();

$('#show-more').click(function(){
    $('#show-more-content').show();
    $('#show-less').show();
    $(this).hide();
});

$('#show-less').click(function(){
    $('#show-more-content').hide();
    $('#show-more').show();
    $(this).hide();
});

// =======================================================================
    // top 10 tab
// =======================================================================
  $(document).ready(function(){
        $('.clickme a').click(function(){
            $('.clickme a').removeClass('activelink');
            $(this).addClass('activelink');
            var tagid = $(this).data('tag');
            $('.list').removeClass('active').addClass('hide');
            $('#'+tagid).addClass('active').removeClass('hide');
        });
    });

// =======================================================================
    // select dropdown
// =======================================================================
(function($) {
          $(document).ready(function() {
            var customSelect = $(".custom-select");

            customSelect.each(function() {
              var thisCustomSelect = $(this),
                options = thisCustomSelect.find("option"),
                firstOptionText = options.first().text();

              var selectedItem = $("<div></div>", {
                class: "selected-item"
              })
                .appendTo(thisCustomSelect)
                .text(firstOptionText);

              var allItems = $("<div></div>", {
                class: "all-items all-items-hide"
              }).appendTo(thisCustomSelect);

              options.each(function() {
                var that = $(this),
                  optionText = that.text();

                var item = $("<div></div>", {
                  class: "item",
                  on: {
                    click: function() {
                      var selectedOptionText = that.text();
                      selectedItem.text(selectedOptionText).removeClass("arrowanim");
                          allItems.addClass("all-items-hide");
                          that.attr("selected", "selected");
                    }
                  }
                })
                  .appendTo(allItems)
                  .text(optionText);
              });
            });

            var selectedItem = $(".selected-item"),
              allItems = $(".all-items");

            selectedItem.on("click", function(e) {
              var currentSelectedItem = $(this),
                currentAllItems = currentSelectedItem.next(".all-items");

              allItems.not(currentAllItems).addClass("all-items-hide");
              selectedItem.not(currentSelectedItem).removeClass("arrowanim");

              currentAllItems.toggleClass("all-items-hide");
              currentSelectedItem.toggleClass("arrowanim");

              e.stopPropagation();
            });

            $(document).on("click", function() {
              var opened = $(".all-items:not(.all-items-hide)"),
                index = opened.parent().index();

              customSelect
                .eq(index)
                .find(".all-items")
                .addClass("all-items-hide");
              customSelect
                .eq(index)
                .find(".selected-item")
                .removeClass("arrowanim");
            });
          });
        })(jQuery);


// =======================================================================
    // select search dropdown
// =======================================================================


$(".default_option").click(function(){
  $(".dropdown ul").addClass("active");
});

$(".dropdown ul li").click(function(){
  var text = $(this).text();
  $(".default_option").text(text);
  $(".dropdown ul").removeClass("active");
});

// =======================================================================
    // upload image 
// =======================================================================
document.querySelectorAll(".drop-zone__input").forEach((inputElement) => {
  const dropZoneElement = inputElement.closest(".drop-zone");

  dropZoneElement.addEventListener("click", (e) => {
    inputElement.click();
  });

  inputElement.addEventListener("change", (e) => {
    if (inputElement.files.length) {
      //updateThumbnail(dropZoneElement, inputElement.files[0]);
    }
  });

  dropZoneElement.addEventListener("dragover", (e) => {
    e.preventDefault();
    dropZoneElement.classList.add("drop-zone--over");
  });

  ["dragleave", "dragend"].forEach((type) => {
    dropZoneElement.addEventListener(type, (e) => {
      dropZoneElement.classList.remove("drop-zone--over");
    });
  });

  dropZoneElement.addEventListener("drop", (e) => {
    e.preventDefault();

    if (e.dataTransfer.files.length) {
      inputElement.files = e.dataTransfer.files;
    //  updateThumbnail(dropZoneElement, e.dataTransfer.files[0]);
    }

    dropZoneElement.classList.remove("drop-zone--over");
  });
});

/**
 * Updates the thumbnail on a drop zone element.
 *
 * @param {HTMLElement} dropZoneElement
 * @param {File} file
 */
function updateThumbnail(dropZoneElement, file) {
  let thumbnailElement = dropZoneElement.querySelector(".drop-zone__thumb");

  // First time - remove the prompt
  if (dropZoneElement.querySelector(".drop-zone__prompt")) {
    dropZoneElement.querySelector(".drop-zone__prompt").remove();
  }

  // First time - there is no thumbnail element, so lets create it
  if (!thumbnailElement) {
    thumbnailElement = document.createElement("div");
    thumbnailElement.classList.add("drop-zone__thumb");
    dropZoneElement.appendChild(thumbnailElement);
  }

  thumbnailElement.dataset.label = file.name;

  // Show thumbnail for image files
  if (file.type.startsWith("image/")) {
    //const reader = new FileReader();

    //reader.readAsDataURL(file);
    //reader.onload = () => {
    //  thumbnailElement.style.backgroundImage = `url('${reader.result}')`;
    //};
  } else {
    thumbnailElement.style.backgroundImage = null;
  }
}

// =======================================================================
    // Languages selected form
// =======================================================================
const _languageSelect= new SlimSelect({
    select: "#multiple"
});
const _categorySelect=new SlimSelect({
    select: "#Category"
});

// =======================================================================
    // password
// =======================================================================

// =======================================================================
    //  
// =======================================================================
// =======================================================================
// slick-carousel
// =======================================================================
$(document).ready(function () {
    $('.customer-logos').slick({
        slidesToShow: 6,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 1500,
        arrows: true,
        dots: false,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>', // Add left arrow
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>', // Add right arrow
        pauseOnHover: false,
        responsive: [{
            breakpoint: 768,
            settings: {
                slidesToShow: 4
            }
        }, {
            breakpoint: 520,
            settings: {
                slidesToShow: 3
            }
        }]
    });
});

// =======================================================================
// accordion faq page
// =======================================================================

const accordionHeaders = document.querySelectorAll(".accordion-header");

accordionHeaders[0].parentElement.classList.add("active");
accordionHeaders[0].nextElementSibling.style.maxHeight = accordionHeaders[0].nextElementSibling.scrollHeight + "px";

function toggleActiveAccordion(e, header) {
    const activeAccordion = document.querySelector(".accordion.active");
    const clickedAccordion = header.parentElement;
    const accordionBody = header.nextElementSibling;

    if (activeAccordion && activeAccordion != clickedAccordion) {
        activeAccordion.querySelector(".accordion-body").style.maxHeight = 0;
        activeAccordion.classList.remove("active");
    }

    clickedAccordion.classList.toggle("active");

    if (clickedAccordion.classList.contains("active")) {
        accordionBody.style.maxHeight = accordionBody.scrollHeight + "px";
    } else {
        accordionBody.style.maxHeight = 0;
    }
}

accordionHeaders.forEach(function (header) {
    header.addEventListener("click", function (event) {
        toggleActiveAccordion(event, header);
    });
});

function resizeActiveAccordionBody() {
    const activeAccordionBody = document.querySelector(".accordion.active .accordion-body");
    if (activeAccordionBody) activeAccordionBody.style.maxHeight = activeAccordionBody.scrollHeight + "px";
}

window.addEventListener("resize", function () {
    resizeActiveAccordionBody();
});











  







