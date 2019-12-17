import React, { Component } from 'react';
import { connect } from 'react-redux';
import './PrivacyPolicyPage.css';

class PrivacyPolicyPage extends Component {

  render() {
    return (

        <html lang="en">
          <head>
            <meta charSet="UTF-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <meta httpEquiv="X-UA-Compatible" content="ie=edge" />
            <link
              rel="stylesheet"
              href="https://use.fontawesome.com/releases/v5.7.2/css/all.css"
              integrity="sha384-fnmOCqbTlWIlj8LyTjo7mOUStjsKC4pOpQbqyi7RrhN7udi9RwhKkMHpvLbHG9Sr"
              crossOrigin="anonymous"
            />
            <title>PDF Viewer</title>
          </head>
          <body>
            <div className="top-bar">
              <div className="top-bar-content">
                <button className="btn" id="prev-page">
                  <i className="fas fa-arrow-circle-left"></i> Predošlá strana
                </button>
                <button className="btn" id="next-page">
                  Nasledujúca strana <i className="fas fa-arrow-circle-right"></i>
                </button>
                <span className="page-info">
                  Strana <span id="page-num"></span> z <span id="page-count"></span>
                </span>
              </div>
            </div>

            <canvas id="pdf-render"></canvas>
            
            <script src="https://mozilla.github.io/pdf.js/build/pdf.js"></script>
            <script src="./PdfHandler.js"></script>
            
          </body>
        </html>

    );
  }
}

export default connect()(PrivacyPolicyPage);
