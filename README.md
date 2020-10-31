# Chameleon
This pckages helps to mange real-time config / feature toggles for MongoDB



<h3 class="code-line" data-line-start=0 data-line-end=1 ><a id="Installation_0"></a>Installation</h3>
<pre><code class="has-line-data" data-line-start="3" data-line-end="5" class="language-sh">dotnet add package Chameleon --version <span class="hljs-number">1.0</span>.<span class="hljs-number">2</span>
</code></pre>
<h3 class="code-line" data-line-start=7 data-line-end=8 ><a id="Usage_7"></a>Usage</h3>
<p class="has-line-data" data-line-start="9" data-line-end="10">Example config class</p>
<pre><code class="has-line-data" data-line-start="11" data-line-end="18" class="language-sh">public class TestConfig
{
    public bool IsEnabledAwesomeFeature { get; <span class="hljs-built_in">set</span>; }
    public string MailSubjectName { get; <span class="hljs-built_in">set</span>; }
}

</code></pre>
<p class="has-line-data" data-line-start="19" data-line-end="20">Example mongodb document representtion</p>
<pre><code class="has-line-data" data-line-start="22" data-line-end="32" class="language-sh">{
    <span class="hljs-string">"_id"</span> : ObjectId(<span class="hljs-string">"5f9d74e89d3a709b9cff38c8"</span>),
    <span class="hljs-string">"UpdatedDate"</span> : ISODate(<span class="hljs-string">"2020-10-31T16:10:22.049Z"</span>),
    <span class="hljs-string">"Config"</span> : {
        <span class="hljs-string">"IsEnabledAwesomeFeature"</span> : <span class="hljs-literal">true</span>,
        <span class="hljs-string">"MailSubjectName"</span> : <span class="hljs-string">"Example mail subject!"</span>
     }
}

</code></pre>
<pre><code class="has-line-data" data-line-start="33" data-line-end="40" class="language-sh">serviceProvider.AddSingleton&lt;IConfigReader&lt;TestConfig&gt;&gt;(new MongoDbConfigReader&lt;TestConfig&gt;(
    <span class="hljs-string">"MongoDbConString"</span>,
    <span class="hljs-string">"MongoDbConfigCollectionName"</span>,
    <span class="hljs-string">"MongoDbConfigDbName"</span>
));

</code></pre>
<p class="has-line-data" data-line-start="41" data-line-end="42">Get config. If config document updates at mongoDb this  will be updates as well</p>
<pre><code class="has-line-data" data-line-start="43" data-line-end="47" class="language-sh">        var configReader = provider.GetService&lt;IConfigReader&lt;TestConfig&gt;&gt;();
        var config = configReader.GetConfig();

</code></pre>
