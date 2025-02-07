
// Upload file functionality 
$(document).ready(function() {

  $('.box').hide();
  $('.box').slideToggle(350);

    $('input[type=file]').wrap('<div class="custom-file"></div>');
    $('.custom-file').append('<label for="file-input"><i class="fa-solid fa-cloud-arrow-up"></i> <span>Upload</span></label>');
    $('.custom-file').append('<span class="file-name"></span>');

    $('.preview-container').hide();
  
    $('.file-name').hide();
  
    $('input[type=file]').change(function() {
      var fileName = $(this).val().split('\\').pop();
      var $fileLabel = $(this).siblings('label');
      var $fileSpan = $fileLabel.find('span');
  
      $('.file-name').show();
      $('.custom-file').addClass('file-added');
      $(this).siblings('.file-name').text(fileName);
  
      if ($(this).get(0).files.length > 0) {
        $fileSpan.text('');
        $fileLabel.find('i.fa-solid.fa-cloud-arrow-up').replaceWith('<i class="fa-regular fa-circle-check text-success"></i>');
      } else {
        $fileSpan.text('Upload');
        $fileLabel.find('i.fa-regular.fa-circle-check.text-success').replaceWith('<i class="fa-solid fa-cloud-arrow-up"></i>');
      }

    
     var file = this.files[0]; 

    if (file && file.type.startsWith('image/')) {
      var reader = new FileReader(); 

      $('.preview-container').show();

      reader.onload = function(e) {
        var fileContents = e.target.result; 

        var previewElement = $('<img>').attr('src', fileContents);
        $('.preview-container').html(previewElement);
      };

      reader.readAsDataURL(file);
    }

    });
  
    $('.custom-file').click(function(event) {
      if ($(event.target).is(this)) {
        $('.preview-container').hide();
        $(this).find('input').click();
      }
    });

    $('.custom-file label').click(function(event) {
        event.preventDefault();
        $('.preview-container').hide();
        $(this).parent().find('input').click();
    });

    $('.custom-file span').click(function(event) {
        event.preventDefault();
        $('.preview-container').hide();
        $(this).parent().find('input').click();
    });


    // Order button animation timeout
    $('.truck-button').click(function() {
      $('input[type=reset]').hide();
      $(this).css({"position": "relative", "top": "20px"});
    
      setTimeout(function() {
        $('.truck-button').css({"position": "", "top": ""});
      }, 4000); 
    });

    // condition box

    //$('.conditionbox').click(function () {
    //    console.log('Testing');
    //    var radio = $(this).find('input[name="QualityCheck"]');
    //    if (radio.length > 0) {
    //      radio.prop('checked', true);
    //        $(this).addClass('active');

    //    } 
    //    $('.conditionbox').not(this).removeClass('active');
    //  });
    

    // animated button

    document.querySelectorAll('.truck-button').forEach(button => {
      button.addEventListener('click', e => {
  
          e.preventDefault();
          
          let box = button.querySelector('.truck-box'),
              truck = button.querySelector('.truck');
          
          if(!button.classList.contains('done')) {
              
              if(!button.classList.contains('animation')) {
  
                  button.classList.add('animation');
  
                  gsap.to(button, {
                      '--truck-box-s': 1,
                      '--truck-box-o': 1,
                      duration: .3,
                      delay: .5
                  });
  
                  gsap.to(box, {
                      x: 0,
                      duration: .4,
                      delay: .7
                  });
  
                  gsap.to(button, {
                      '--hx': -5,
                      '--bx': 50,
                      duration: .18,
                      delay: .92
                  });
  
                  gsap.to(box, {
                      y: 0,
                      duration: .1,
                      delay: 1.15
                  });
  
                  gsap.set(button, {
                      '--truck-y': 0,
                      '--truck-y-n': -26
                  });
  
                  gsap.to(button, {
                      '--truck-y': 1,
                      '--truck-y-n': -25,
                      duration: .2,
                      delay: 1.25,
                      onComplete() {
                          gsap.timeline({
                              onComplete() {
                                  button.classList.add('done');
                              }
                          }).to(truck, {
                              x: 0,
                              duration: .4
                          }).to(truck, {
                              x: 40,
                              duration: 1
                          }).to(truck, {
                              x: 20,
                              duration: .6
                          }).to(truck, {
                              x: 96,
                              duration: .4
                          });
                          gsap.to(button, {
                              '--progress': 1,
                              duration: 2.4,
                              ease: "power2.in"
                          });
                      }
                  });
                  
              }
              
          } else {
              button.classList.remove('animation', 'done');
              gsap.set(truck, {
                  x: 4
              });
              gsap.set(button, {
                  '--progress': 0,
                  '--hx': 0,
                  '--bx': 0,
                  '--truck-box-s': .5,
                  '--truck-box-o': 0,
                  '--truck-y': 0,
                  '--truck-y-n': -26
              });
              gsap.set(box, {
                  x: -24,
                  y: -6
              });
          }
  
      });
  });
  
    
  
  });

$(window).on('load', function () {
    $('#loader').hide();
});

$(document).ajaxStart(function () {
    $("#loader").show();
});

$(document).ajaxStop(function () {
    $("#loader").hide();
});