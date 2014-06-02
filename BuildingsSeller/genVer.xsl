<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text"/>
	<xsl:template match="/">

#define VER	<xsl:value-of select="build/major"/>,<xsl:value-of select="build/minor"/>,<xsl:value-of select="build/build"/>,<xsl:value-of select="build/release"/>

#define STRVER 	&quot;<xsl:value-of select="build/major"/>,<xsl:value-of select="build/minor"/>,<xsl:value-of select="build/build"/>,<xsl:value-of select="build/release"/>&quot;

#define COMPANYNAME &quot;<xsl:value-of select="build/companyname"/>&quot;

#define COPYRIGHT &quot;<xsl:value-of select="build/copyright"/>&quot;
		
	</xsl:template>
</xsl:stylesheet>