<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <title>Renegade X Stats</title>
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <link rel="stylesheet" href="/css/bootstrap.min.css" media="screen">
        <link rel="stylesheet" href="/css/custom.css" media="screen">
    </head>
    <body>     
        <div class="container">
            <div class="navbar navbar-inverse" style="margin-top:20px">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-responsive-collapse">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="{{ route('home') }}">Renegade X Stats</a>
                </div>
                <div class="navbar-collapse collapse navbar-responsive-collapse">
                    <ul class="nav navbar-nav">
                        <li{{ Request::is('/') ? ' class="active"' : '' }}><a href="{{ route('home') }}">Winners</a></li>
                        <li{{ Request::is('losers') ? ' class="active"' : '' }}><a href="{{ route('losers') }}">Losers</a></li>
                    </ul>
                    @if(Auth::check())
                        <ul class="nav navbar-nav navbar-right">
                            <li class="dropdown"><a href="#" class="dropdown-toggle" data-toggle="dropdown">Account
                                <b class="caret"></b></a>
                                <ul class="dropdown-menu">
                                    <li>
                                        <div class="navbar-content">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <img src="{{ Auth::getUser()->hasPhoto() ? Auth::getUser()->getPhoto() : '/photo.jpg'}}"
                                                        alt="Alternate Text" class="img-responsive" />
                                                    <p></p>
                                                </div>
                                                <div class="col-md-7">
                                                    <span>{{ Auth::getUser()->name }}</span>
                                                    <p class="text-muted small">
                                                        {{ Auth::getUser()->email }}
                                                    </p>
                                                    <div class="divider">
                                                    </div>
                                                    <a href="{{ route('player', Auth::getUser()->steamid) }}" class="btn btn-primary btn-sm active">View Profile</a>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="navbar-footer">
                                            <div class="navbar-footer-content">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <a href="{{ route('editprofile', Auth::getUser()->steamid) }}" class="btn btn-default btn-sm">Change Settings</a>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <a href="{{ route('logout') }}" class="btn btn-default btn-sm pull-right">Sign Out</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    @else
                        <ul class="nav navbar-nav navbar-right">
                            <li><a href="{{ route('login') }}"><img src="/steam.png" /></a></li>
                        </ul>
                    @endif
                    <form class="navbar-form navbar-right" action="{{ route('search') }}">
                        <input type="text" name="name" class="form-control col-lg-8" placeholder="Search for player...">
                    </form>
                    <ul class="nav navbar-nav navbar-right" style="margin-top:15px; margin-right:10px;">
                        <li><p><strong>Join our server @ vicious-reality.com:7777</strong></p></li>
                    </ul>
                </div>
            </div>
            @include('messages')
            <div class="row">
                <div class="col-lg-12">
                    <div class="alert alert-dismissable alert-warning">
                        <button type="button" class="close" data-dismiss="alert">X</button>
                        <h4>Notice!</h4>
                        <p>Hey, glad you're checking out our server. Just remember that you'll need to be logged into Steam when playing on our server to have your stats tracked!</p>
                    </div>
                </div>
            </div>
            @yield('content')
        </div>
        <script type="text/javascript" src="/js/jquery.min.js"></script>
        <script type="text/javascript" src="/js/bootstrap.min.js"></script>
        @yield('scripts')
    </body>
</html>
