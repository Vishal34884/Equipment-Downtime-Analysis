<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Equipment._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/owl.carousel.min.css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/OwlCarousel2/2.3.4/assets/owl.carousel.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/OwlCarousel2/2.3.4/assets/owl.theme.default.min.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/OwlCarousel2/2.3.4/owl.carousel.min.js"></script>
    <!--Script for slideshow-->
    <script>
        $(document).ready(function () {
            $(".owl-carousel").owlCarousel({
                items: 1,
                autoplay: true,
                autoplaySpeed: 2000,
                loop: true,
                nav: true,
                navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
                dots: false 
            });
        });
    </script>
    <style>
        .carousel-caption {
            position: absolute;
            top: 55%;
            left: 35%;
            transform: translate(-50%, -50%);
        }

        .display-3 {
            white-space: nowrap;
        }

        .img {
            box-shadow: inherit;
        }

        .owl-theme .owl-nav [class*=owl-] {
            color: white;
        }

        .owl-carousel {
            display: flex;
            justify-content: center;
            padding-left: 200px;
        }
    </style>
    <br />
    <br />
    <!-- Image Start -->
    <div class="container-fluid p-0 pb-5 mb-5">
        <div id="header-carousel" class="carousel slide carousel-fade" data-ride="carousel">
            <div class="carousel-inner" style="border-radius: 1rem">
                <div class="carousel-item active" style="min-height: 100%;">
                    <div class="position-relative">
                        <img class="img position-absolute top-0 start-0 w-100" src="image/dwntm.jpg" style="height: 500px; width: 100%; background-size: cover; object-fit: cover;">
                        <div class="overlay position-absolute top-0 start-0 w-100 h-100" style="background-color: rgba(0, 0, 0, 0.6);"></div>
                    </div>
                    <div class="carousel-caption d-flex align-items-center justify-content-center">
                        <div class="p-5 center" style="width: 100%; padding-left: 40px;">
                            <h1 class="display-3 text-white mb-md-4" style="font-size: 60px;"><b>Equipment Downtime Analysis</b></h1>
                            <br />
                            <div style="padding-left: 350px;">
                                <a runat="server" href="~/About" class="btn btn-primary">Go to Downtime Analysis</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--Image End-->
    <br />
    <br />
    <!--Slideshow Start-->
    <div class="owl-carousel">
        <div class="item">
            <div class="item">
                <div class="container" data-aos="fade-up" data-aos-delay="100">
                    <div class="row gy-3">
                        <div class="col-lg-4" style="background-color: white; border-top-left-radius: 1rem; border-bottom-left-radius: 1rem">
                            <div class="info-item  d-flex flex-column justify-content-center align-items-center">
                                <br />
                                <h3>MES</h3>
                                <p>
                                    Manufacturing Execution System is a software-based system that is designed to track and manage the production process on the factory floor. 
                                    MES software provides real-time information about the production process, including data on materials, labor, and equipment, which can be used to improve efficiency, reduce costs, and increase quality.
                                    MES typically integrates with other enterprise software systems, such as enterprise resource planning (ERP) and product lifecycle management (PLM).
                                </p>
                            </div>
                        </div>
                        <div class="col-lg-4" style="background-color: white; border-top-right-radius: 1rem; border-bottom-right-radius: 1rem">
                            <div class="info-item  d-flex flex-column justify-content-center align-items-center">
                                <br />
                                <img src="/Image/MES.jfif" style="height: 267px; width: 100%" />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="item">
            <div class="item">
                <div class="container" data-aos="fade-up" data-aos-delay="100">
                    <div class="row gy-3">
                        <div class="col-lg-4" style="background-color: white; border-top-left-radius: 1rem; border-bottom-left-radius: 1rem">
                            <div class="info-item  d-flex flex-column justify-content-center align-items-center">
                                <br />
                                <h3>Equipment</h3>
                                <p>
                                    Equipment refers to the tools, machinery, devices, or other physical objects used to perform a specific task or activity. This can range from simple hand tools, such as hammers and screwdrivers, to complex machinery, such as computers and heavy construction equipment. 
                                    Equipment is typically designed to enhance or facilitate a specific process or task and can be used in a wide variety of settings, such as manufacturing, construction, medicine, sports, and entertainment.
                                     <br />
                                    <br />
                                </p>
                            </div>
                        </div>
                        <div class="col-lg-4" style="background-color: white; border-top-right-radius: 1rem; border-bottom-right-radius: 1rem">
                            <div class="info-item  d-flex flex-column justify-content-center align-items-center">
                                <br />
                                <img src="/Image/equipment.jfif" style="height: 267px; width: 100%" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="item">
            <div class="container" data-aos="fade-up" data-aos-delay="100">
                <div class="row gy-3">
                    <div class="col-lg-4" style="background-color: white; border-top-left-radius: 1rem; border-bottom-left-radius: 1rem">
                        <div class="info-item  d-flex flex-column justify-content-center align-items-center">
                            <br />
                            <h3>Equipment Downtime Analysis</h3>
                            <p>
                                Equipment Downtime Analysis (EDA) is a process used to identify and analyze the reasons for equipment downtime, which is the time during which a piece of equipment is not operational. 
                                EDA aims to determine the root causes of downtime, so that steps can be taken to minimize or eliminate it.
                                EDA involves collecting data on the duration and frequency of downtime, as well as the reasons for it. This data can be analyzed to identify patterns and trends, and to determine which issues are most common and impactful.
                            </p>
                        </div>
                    </div>
                    <div class="col-lg-4" style="background-color: white; border-top-right-radius: 1rem; border-bottom-right-radius: 1rem">
                        <div class="info-item  d-flex flex-column justify-content-center align-items-center">
                            <br />
                            <img src="/Image/downtime.jfif" style="height: 267px; width: 100%;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--Slideshow End-->
    </div>
</asp:Content>
