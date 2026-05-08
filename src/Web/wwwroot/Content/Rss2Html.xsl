<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl = "http://www.w3.org/1999/XSL/Transform" xmlns = "http://www.w3.org/1999/xhtml">
  <xsl:output method="html" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" />
  <xsl:variable name="godecoding">go_decoding();</xsl:variable>
  <xsl:variable name="title" select="/rss/channel/title" />
  <xsl:variable name="feedUrl" select="/rss/channel/atom10:link[@rel='self']/@href" xmlns:atom10="http://www.w3.org/2005/Atom" />
  <xsl:template match="/">
    <xsl:element name="html">
      <head>
        <title>
          <xsl:value-of select="$title" />
        </title>
        <xsl:element name="meta">
          <xsl:attribute name="content-type">text/html; charset=iso-8859-1</xsl:attribute>
        </xsl:element>
        <xsl:element name="link">
          <xsl:attribute name="rel">stylesheet</xsl:attribute>
          <xsl:attribute name="href">http://www.alwaysmoveforward.com/Content/Rss.css</xsl:attribute>
          <xsl:attribute name="type">text/css</xsl:attribute>
        </xsl:element>
        <link rel="alternate" type="application/rss+xml">
            <xsl:attribute name="href">
              <xsl:value-of select="$title" />
            </xsl:attribute>
          <xsl:attribute name="title">
            <xsl:value-of select="$title" />
          </xsl:attribute>
        </link>
      </head>
      <body>
        <div class="feedBody">
          <xsl:for-each select="/rss/channel">
            <p>
              <h1 class="feedtitle">
                <a accesskey="0" href="{link}">
                  <xsl:value-of select="./title"/>
                </a>
              </h1>
            </p>
            <xsl:if test="description != ./title" >
              <p class='desc'>
                <xsl:value-of select="description"/>
              </p>
            </xsl:if>
            <xsl:variable name="itemCount" select="count(item)" />
            <p class='leadIn'>
              <xsl:choose>
                <xsl:when test="$itemCount = 0" >No items </xsl:when>
                <xsl:when test="$itemCount = 1" >The only item </xsl:when>
                <xsl:otherwise>
                  The <xsl:value-of select="$itemCount" /> items
                </xsl:otherwise>
              </xsl:choose>
              currently in this feed:
            </p>
            <dl class='Items'>
              <xsl:if test='$itemCount = 0'>
                <dt>(Empty)</dt>
              </xsl:if>
              <xsl:for-each select="item">
                <div class="channelItem">
                  <div class="channelItemTitle">
                    <a href="{link}">
                      <xsl:choose>
                        <xsl:when test="not(title) or title = ''" >
                          <em>(No title)</em>
                        </xsl:when>
                        <xsl:otherwise		>
                          <xsl:value-of select="title"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </a>
                  </div>
                  <div style="channelItemAuthor">
                    Posted by <xsl:value-of select="./author"/> on <xsl:value-of select="./pubDate"/>
                  </div>
                  <br/>
                  <div class="channelItemDescription">
                    <xsl:if test="description" >
                      <xsl:value-of  disable-output-escaping="yes" select="description" />
                    </xsl:if>
                  </div>
                </div>
                <br/>
                <br/>
              </xsl:for-each>
            </dl>
            <p class='end'></p>
          </xsl:for-each>
        </div>
      </body>
    </xsl:element>
  </xsl:template>
</xsl:stylesheet>
