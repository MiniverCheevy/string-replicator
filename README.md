string-replicator
=================

Simple templating tool to pass csv data through a template to generate text.  Takes csv data, splits it by line and applies the specified format to each line.  It uses a modified version of <a href='https://github.com/mono/mono/blob/effa4c07ba850bedbe1ff54b2a5df281c058ebcb/mcs/class/corlib/System/String.cs' target='_blank'>String.Format</a> from the mono project.  An attempt is made to coerce the csv data into .net data types so that data type specific formatting options (ex: yyyy for year in a date) work as expected.  There are also a few additional formatting options:
<br/><br/>
{0:!} Friendly Name<br/>
{0:^} Proper Case<br/>
{#} 0 based row number<br/>
{+} 1 based row number<br/>
<br/>
<br/>
<!-- choco install string-replicator -->

